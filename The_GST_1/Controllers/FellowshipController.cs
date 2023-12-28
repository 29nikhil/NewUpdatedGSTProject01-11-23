using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository_Logic.Dto;
using Repository_Logic.ErrorLogsRepository.Interface;
using Repository_Logic.FellowshipRepository.Interface;
using Repository_Logic.ModelView;
using Repository_Logic.UserOtherDatails.implementation;
using Repository_Logic.UserOtherDatails.Interface;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Web.Providers.Entities;

namespace The_GST_1.Controllers
{
    [Authorize]
    [Authorize(Roles = "Fellowship,CA")]

    public class FellowshipController : Controller
    {
        private readonly IErrorLogs _errorLogs;
        private readonly IExtraDetails extraDetails;
        private readonly Application_Db_Context _context;
        private readonly IFellowshipRepository _fellowshipRepository;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;

        public FellowshipController(IExtraDetails extraDetails, IFellowshipRepository fellowshipRepository, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> IdentityUserManager, IErrorLogs errorLogs)
        {
            _errorLogs = errorLogs;
            this.extraDetails = extraDetails;
            _fellowshipRepository = fellowshipRepository;
            _IdentityUserManager = IdentityUserManager;
        }
        [Authorize(Roles = "CA")]
        public IActionResult FellowshipList()
        {
            try
            {
               
                var UserData = _fellowshipRepository.GetAllFellowshipRecord();

                return View(UserData);
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "AN ERROR OCCURRED WHILE LOADING ALL FELLOWSHIP DETAILS.";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });


            }
        } //Fellowship List

        public async Task<IActionResult> GetFellowship(string id)
        {
            try
            {
                
                var user = _fellowshipRepository.GetFellowShipṚeccord(id);
                ViewBag.Country = user.Country;
                ViewBag.Email = user.Email;
                return View(user);
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "AN ERROR OCCURRED WHILE LOADING FELLOWSHIP DETAILS FOR UPDATING. ";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });
            }
        }//Get Fellowship Data from Ca Side Delete or Update

        public IActionResult UpdateFelloshipProfile(Application_User_Dto user) //Update Fellowship Profile Method Submit form
        {
            try
            {
                
                var useremailcheck = _fellowshipRepository.GetFellowShipṚeccord(user.Id);
                _fellowshipRepository.UpdateFellowship(user);
                TempData["ProfileUpdated"] = "Profile updated successfully";
                var UserData = _fellowshipRepository.GetAllFellowshipRecord();
                return RedirectToAction("GetYourProfile");
            }
            catch (Exception ex) {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);

                var errorMessage = "AN ERROR OCCURRED WHILE UPDATING DETAILS.";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });

            }
        }
        public async Task<IActionResult> GetFellowshipView(string id)
        {
            try
            {
                
                if (id == null)
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    var user = _fellowshipRepository.GetFellowShipṚeccord(userId);
                    return View(user);

                }
                else
                {
                    var user = _fellowshipRepository.GetFellowShipṚeccord(id);
                    return View(user);

                }
            }
            catch(Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "AN ERROR OCCURRED WHILE LOADING FELLOWSHIP DETAILS.";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });



            }
        }  //Show Details Fellowship From Ca side view

        public async Task<IActionResult> GetYourProfile()  //Get Fellowship Self Profile View
        {
            try
            {
                
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var user = await _IdentityUserManager.FindByIdAsync(userId);

                var isInRole = await _IdentityUserManager.IsInRoleAsync(user, "Fellowship");
                var user1 = _fellowshipRepository.GetFellowShipṚeccord(userId);

                if (isInRole)
                {
                    ViewBag.UserProfileType = "FellowShip Profile";
                    return View(user1);

                }
                else
                {
                    ViewBag.UserProfileType = "CA Profile";

                    return View(user1);

                }
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "AN ERROR OCCURRED WHILE LOADING PROFILE PAGE.";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });


            }
        }

        public IActionResult UpdateFellowship(Application_User_Dto user) //Update CA Side Update FellowShip From Submit Method
        {


            try
            {
                
                var useremailcheck = _fellowshipRepository.GetFellowShipṚeccord(user.Id);
                if (useremailcheck.Email != user.Email)
                {


                    _fellowshipRepository.UpdateFellowship(user);
                    TempData["UpdateFellowship"] = "Update Fellowship Record:" + user.FirstName;
                    var UserData = _fellowshipRepository.GetAllFellowshipRecord();
                    return RedirectToAction("FellowshipList", "Fellowship", UserData);



                }
                else
                {
                    _fellowshipRepository.UpdateFellowship(user);
                    TempData["UpdateFellowship"] = "Update Fellowship Record:" + user.FirstName;
                    var UserData = _fellowshipRepository.GetAllFellowshipRecord();
                    return RedirectToAction("FellowshipList", "Fellowship", UserData);

                }



            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "AN ERROR OCCURRED WHILE UPDATING DETAILS.";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });


            }



        }
        public async Task<IActionResult> UpdateYourProfile() //Fellowship Profile Update View 
        {

            try
            {
               
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var user = _fellowshipRepository.GetFellowShipṚeccord(userId);
                ViewBag.Email = user.Email;
                ViewBag.UserProfileUpdate = "Update Your Profile";
                return View(user);
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "AN ERROR OCCURRED WHILE LOADING DETAILS FOR UPDATING.";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });

            }
        }







        //    public IActionResult ViewProfileFellowship()
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    var UserData = _fellowshipRepository.GetFellowShipṚeccord(userId);
        //    return View(UserData);
        //}

        public async Task<IActionResult> DeleteFellowship(string id)
        {
            try
            {
                
                _fellowshipRepository.DeleteFellowship(id);
                var UserData = _fellowshipRepository.GetAllFellowshipRecord();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "AN ERROR OCCURRED WHILE DELETING THE FELLOWSHIP.";
                return Json(new { success = false, message = errorMessage });


            }


        }

        public async Task<JsonResult> GetFellowshipList()

        {

            var dataTable_ = new DataTable_Dto
            {
                Draw = Request.Form["draw"].FirstOrDefault(),
                sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault(),
                sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault(),
                SearchValue = Request.Form["search[value]"].FirstOrDefault(),
                PageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0"),
                Skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0"),
            };

            dataTable_.TotalRecord = dataTable_.FilterRecord = _fellowshipRepository.GetAllFellowshipRecord().Count();

            var FellowshipList = _fellowshipRepository.GetAllFellowshipRecord(dataTable_);

            var returnObj = new
            {
                draw = dataTable_.Draw,
                recordsTotal = dataTable_.TotalRecord,
                recordsFiltered = dataTable_.FilterRecord,
                data = FellowshipList
            };

            return Json(returnObj);

        }





    }
}