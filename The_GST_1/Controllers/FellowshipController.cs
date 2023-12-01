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

        public FellowshipController(IExtraDetails extraDetails, IFellowshipRepository fellowshipRepository)
        {
            this.extraDetails = extraDetails;
            _fellowshipRepository = fellowshipRepository;
        }
        [Authorize(Roles = "CA")]
        public IActionResult FellowshipList()
        {
            var UserData = _fellowshipRepository.GetAllFellowshipRecord();

            return View(UserData);
        }

        public async Task<IActionResult> GetFellowship(string id)
        {
            var user =  _fellowshipRepository.GetFellowShipṚeccord(id);
            @ViewBag.Country = user.Country;
            return View(user);
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

        }

        public IActionResult UpdateFellowship(Application_User user)
        {
            var useremailcheck = _fellowshipRepository.GetFellowShipṚeccord(user.Id);

       

                _fellowshipRepository.UpdateFellowship(user);
                TempData["UpdateFellowship"] = "Update Fellowship Record:" + user.FirstName;
                var UserData = _fellowshipRepository.GetAllFellowshipRecord();
                return RedirectToAction("FellowshipList", "Fellowship", UserData);

               

            






        }

        public IActionResult ViewProfileFellowship()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var UserData = _fellowshipRepository.GetFellowShipṚeccord(userId);
            return View(UserData);
        }

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
