using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Mvc;
using Repository_Logic.Dto;
using Repository_Logic.ErrorLogsRepository.Interface;
using Repository_Logic.FellowshipRepository.Interface;
using System.Security.Claims;

namespace The_GST_1.Controllers
    {
        public class CAController : Controller
        {
             private readonly IErrorLogs _errorLogs;
             private readonly IFellowshipRepository _fellowshipRepository;
            public CAController(IFellowshipRepository fellowshipRepository, IErrorLogs errorLogs)
            {
                  _errorLogs= errorLogs;
                _fellowshipRepository = fellowshipRepository;

            }

            public async Task<IActionResult> GetCAProfile()
            {

             try
             {
               
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var CADetails = _fellowshipRepository.GetFellowShipṚeccord(userId);
                return View(CADetails);
             }
               catch (Exception ex)
              {

                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "An error occurred while loading CA profile details";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });


               }
            
             }

            public async Task<IActionResult> UpdateCAProfile()
            {
            try
            {
               
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var user = _fellowshipRepository.GetFellowShipṚeccord(userId);
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
                var errorMessage = "An error occurred while loading CA details page for editing";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });

            }

            }
            public IActionResult UpdateProfile(Application_User_Dto user)
            {
            try
            {
               
                var useremailcheck = _fellowshipRepository.GetFellowShipṚeccord(user.Id);
                _fellowshipRepository.UpdateFellowship(user);
                TempData["ProfileUpdated"] = "Profile updated successfully";
                var UserData = _fellowshipRepository.GetAllFellowshipRecord();
                return RedirectToAction("UpdateCAProfile");
            }
            catch(Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "An error occurred while updating profile details";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });

            }
         }
        }
    }

