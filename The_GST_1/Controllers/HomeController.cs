using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using Repository_Logic.Dto;
using Repository_Logic.FellowshipRepository.Interface;
using Repository_Logic.GlobalFunction.Interface;
using Repository_Logic.ReturnFile.Interface;
using Repository_Logic.UserOtherDatails.Interface;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Web.Providers.Entities;
using The_GST_1.Models;

namespace The_GST_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<Microsoft.AspNetCore.Identity.IdentityUser> _signInManager;
        private readonly UserManager<Microsoft.AspNetCore.Identity.IdentityUser> _userManager;
        private readonly IExtraDetails _extraDetails;
        private readonly IFellowshipRepository _fellowship;
        private readonly Application_Db_Context _context;
        private readonly IGlobalFunctionRepository _globalFunctionRepository;
        private readonly IReturnFile _returnFile;
        public HomeController(ILogger<HomeController> logger, SignInManager<Microsoft.AspNetCore.Identity.IdentityUser> signInManager,
        UserManager<Microsoft.AspNetCore.Identity.IdentityUser> userManager, IExtraDetails extraDetails, IFellowshipRepository fellowship, Application_Db_Context context , IGlobalFunctionRepository globalFunctionRepository, IReturnFile returnFile)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _extraDetails = extraDetails;
            _fellowship = fellowship;
            _context = context;
            _globalFunctionRepository = globalFunctionRepository;
            _returnFile = returnFile;
           

        }
        [Authorize]
        public  async Task<IActionResult> Index (string id)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           var DoneChange =_globalFunctionRepository.UserSideDoneChangesFile(userId);
            int UserUploadFiles = _globalFunctionRepository.UserSideUploadFiles(userId);

            float per1 = UserUploadFiles > 0 ? (DoneChange * 100.0f / UserUploadFiles) : 0.0f;
            ViewBag.UserSideDoneFile = (int)per1;

            //FellowshipReturnFileCount
                
            var DoneChangeFellowship = _globalFunctionRepository.FellowshipAllowcatedTask(userId);
            int UserUploadFilesFellowship = _globalFunctionRepository.FellowshipTotalFileUploded(userId);

            float perFellow = UserUploadFiles > 0 ? (DoneChangeFellowship * 100.0f / UserUploadFilesFellowship) : 0.0f;
            ViewBag.FellowSideDoneFile = (int)perFellow;



            ViewBag.ReturnFile = _globalFunctionRepository.ReturnFileCountAll();
            ViewBag.UserReturnFiles = ReturnFileCountUser();
            var DoneTask = _globalFunctionRepository.AllowcatedTaskDone();
            var TotalTask = _globalFunctionRepository.AllowcatedTaskTotal();

            float per = TotalTask > 0 ? (DoneTask * 100.0f / TotalTask) : 0.0f;
            ViewBag.TotalTaskCompleted =(int) per;
            ViewBag.AllFellowship = TempData["TotalFellowship"];
          
            int TotalFellowship = await _globalFunctionRepository.TotalFellowship();
            int TotalUser = await _globalFunctionRepository.TotalUser();
            ViewBag.Individual = _globalFunctionRepository.IndividualUserType();
            ViewBag.Oraganization=_globalFunctionRepository.OrganizationUserType();
            //ViewBag.Oraganization = _globalFunctionRepository.OrganizationUserType();

            ViewBag.AllFellowship= TotalFellowship;
            ViewBag.AllUser = TotalUser;

            ViewBag.TotalMonthlyGst = _globalFunctionRepository.MonthlyGst();
            ViewBag.TotalYearlyGst = _globalFunctionRepository.YearlyGst();



            return View();

        }



        int ReturnFileCountUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
               int FileCount = _globalFunctionRepository.UserSideReturnFileCount(userId);

                if (FileCount != null)
                {
                    
                    return FileCount;
                }
            }
            return 0;
        }


        [Authorize]
        public async Task<IActionResult> GetUserName()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                var userData = await _globalFunctionRepository.GetUserDataNavBar(userId);

                if (userData != null)
                {
                    var getDataUser = userData;

                    TempData["UserName"] = getDataUser.FirstName;

                    return Json(getDataUser.FirstName);
                }
            }
            return Json(null);

            // Return data as JSON
        }

        [Authorize]
        public async Task<List<object>> GetListChartAsync()
        {

            
           
            List<object> datas = new List<object>();
            List<string> labels = new List<string> { "Confirm", "NotConfirm", "TotalUser" };
            datas.Add(labels);
            List<int> UserCount = new List<int>();
            var Confirm=await _extraDetails.GetUserEmailConfirmCountAll();
            var NotConfirma1 =await _extraDetails.GetUserNotConfirmedcountAll();


            UserCount.Add(Confirm);
            UserCount.Add(NotConfirma1);
            UserCount.Add(Confirm + NotConfirma1);
            datas.Add(UserCount);
            return datas;
        }
        [Authorize]
        public async Task<List<object>> GetListChartUser()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int FileCountReturn = _globalFunctionRepository.UserSideReturnFileCount(userId);
            int PendingChangesUserFile=_globalFunctionRepository.UserSidePendingChagesFile(userId);
            int UserUploadFiles=_globalFunctionRepository.UserSideUploadFiles(userId);
            List<object> datas = new List<object>();
            List<string> labels = new List<string> { "UploadFile", "ReturnFile", "PendingChanges" };
            datas.Add(labels);
            List<int> UserCount = new List<int>();
           


            UserCount.Add(UserUploadFiles);
            UserCount.Add(FileCountReturn);
            UserCount.Add(PendingChangesUserFile);
            datas.Add(UserCount);
            return datas;
        }



        public IActionResult Privacy()
        {


          
                if (_signInManager.IsSignedIn(User))
                {
                    // User is signed in, perform redirection to a different action or URL
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // If the user is not signed in, redirect to the Identity Login page
                    string url = "Identity/Account/Login";
                    return Redirect(url);
                }
              
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
