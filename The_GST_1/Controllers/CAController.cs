using Microsoft.AspNetCore.Mvc;
using Repository_Logic.Dto;
using Repository_Logic.FellowshipRepository.Interface;
using System.Security.Claims;

namespace The_GST_1.Controllers
    {
        public class CAController : Controller
        {
            private readonly IFellowshipRepository _fellowshipRepository;
            public CAController(IFellowshipRepository fellowshipRepository)
            {

                _fellowshipRepository = fellowshipRepository;

            }

            public async Task<IActionResult> GetCAProfile()
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


                var CADetails = _fellowshipRepository.GetFellowShipṚeccord(userId);
                return View(CADetails);

            }

            public async Task<IActionResult> UpdateCAProfile()
            {

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var user = _fellowshipRepository.GetFellowShipṚeccord(userId);
                ViewBag.UserProfileUpdate = "Update Your Profile";
                return View(user);


            }
            public IActionResult UpdateProfile(Application_User_Dto user)
            {
                var useremailcheck = _fellowshipRepository.GetFellowShipṚeccord(user.Id);
                _fellowshipRepository.UpdateFellowship(user);
                TempData["ProfileUpdated"] = "Profile updated successfully";
                var UserData = _fellowshipRepository.GetAllFellowshipRecord();
                return RedirectToAction("UpdateCAProfile");
            }
        }
    }

