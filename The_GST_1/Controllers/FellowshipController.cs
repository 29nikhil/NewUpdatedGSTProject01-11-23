using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository_Logic.Dto;
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
        private readonly IExtraDetails extraDetails;
        private readonly Application_Db_Context _context;
        private readonly IFellowshipRepository _fellowshipRepository;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;

        public FellowshipController(IExtraDetails extraDetails, IFellowshipRepository fellowshipRepository, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> IdentityUserManager)
        {
            this.extraDetails = extraDetails;
            _fellowshipRepository = fellowshipRepository;
            _IdentityUserManager = IdentityUserManager;
        }
        [Authorize(Roles = "CA")]
        public IActionResult FellowshipList()
        {
            var UserData = _fellowshipRepository.GetAllFellowshipRecord();

            return View(UserData);
        } //Fellowship List

        public async Task<IActionResult> GetFellowship(string id)
        {
            var user =  _fellowshipRepository.GetFellowShipṚeccord(id);
            ViewBag.Country = user.Country;
            ViewBag.Email=user.Email;
            return View(user);
        }//Get Fellowship Data from Ca Side Delete or Update

        public IActionResult UpdateFelloshipProfile(Application_User_Dto user) //Update Fellowship Profile Method Submit form
        {
            var useremailcheck = _fellowshipRepository.GetFellowShipṚeccord(user.Id);
            _fellowshipRepository.UpdateFellowship(user);
            TempData["ProfileUpdated"] = "Profile updated successfully";
            var UserData = _fellowshipRepository.GetAllFellowshipRecord();
            return RedirectToAction("GetYourProfile");
        }
        public async Task<IActionResult> GetFellowshipView(string id)
        {
            if(id == null)
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

        }  //Show Details Fellowship From Ca side view

        public async Task<IActionResult> GetYourProfile()  //Get Fellowship Self Profile View
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

        public IActionResult UpdateFellowship(Application_User_Dto user) //Update CA Side Update FellowShip From Submit Method
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
        public async Task<IActionResult> UpdateYourProfile() //Fellowship Profile Update View 
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = _fellowshipRepository.GetFellowShipṚeccord(userId);
            ViewBag.UserProfileUpdate = "Update Your Profile";
            return View(user);


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

                return RedirectToAction("FellowshipList", "Fellowship", UserData);
            }
            catch (Exception ex)
            {
                throw new Exception();
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
