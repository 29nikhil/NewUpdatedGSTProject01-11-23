using Microsoft.AspNetCore.Mvc;
using Repository_Logic.DeleteLogsRepository.Interface;
using Repository_Logic.Dto;
using Repository_Logic.LoginLogsDataRepository.Interface;
using Repository_Logic.RegistorLogsRepository.Interface;
using Repository_Logic.TaskAllocation.Implementation;
using System.Linq;
using System.Security.Claims;

namespace The_GST_1.Controllers
{
    public class Log_InformationController : Controller
    {

         private readonly IRegisterLogs _registerLogs;
         private readonly ILoginLogs _loginLogs;
         private readonly IDeleteLogs _deleteLogs;
        public Log_InformationController(IRegisterLogs registerLogs,ILoginLogs loginLogs, IDeleteLogs deleteLogs)
        {
            _loginLogs = loginLogs;
           _registerLogs = registerLogs;
            _deleteLogs = deleteLogs;
        }

        public IActionResult RegisterLogView()
        {

            var log = _registerLogs.GetAllRegistorLogs().ToList();
            
            
            return View(log);
        }
        public IActionResult RegisterLogViewFellowship()
        {

            var log = _registerLogs.GetAllRegistorLogs().ToList();


            return View(log);
        }


        public async Task<JsonResult> ResistorlogListDataTable()
        {
            var LoginSessionID = "null";
            var dataTable_ = new DataTable_Dto
            {
                Draw = Request.Form["draw"].FirstOrDefault(),
                sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault(),
                sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault(),
                SearchValue = Request.Form["search[value]"].FirstOrDefault(),
                PageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0"),
                Skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0")
            };

            List<RegisterLogVIew_Dto> Records = await _registerLogs.ViewRegisterLogsDataTable(dataTable_);
            var totalRecord = Records.Count();
            var filterRecord = Records.Count();
            var FellowshipList = Records.Skip(dataTable_.Skip).Take(dataTable_.PageSize).ToList();
            var returnObj = new
            {
                draw = dataTable_.Draw,
                recordsTotal = totalRecord,
                recordsFiltered = filterRecord,
                data = FellowshipList
            };
            return Json(returnObj);


        }


        public IActionResult LoginLogs()
        {

            var LoginLogs = _loginLogs.GetLoginLogs();

            return View("LoginLogsView", LoginLogs);
        }

        public JsonResult LoginLogsDataTable()
        {

            var dataTable_ = new DataTable_Dto
            {
                Draw = Request.Form["draw"].FirstOrDefault(),
                sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault(),
                sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault(),
                SearchValue = Request.Form["search[value]"].FirstOrDefault(),
                PageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0"),
                Skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0")
            };

            List<LoginLogs_Dto> Records = _loginLogs.ViewLoginLogsDataTable(dataTable_);
            var totalRecord = Records.Count();
            var filterRecord = Records.Count();
            var FellowshipList = Records.Skip(dataTable_.Skip).Take(dataTable_.PageSize).ToList();
            var returnObj = new
            {
                draw = dataTable_.Draw,
                recordsTotal = totalRecord,
                recordsFiltered = filterRecord,
                data = FellowshipList
            };
            return Json(returnObj);
        }



        public IActionResult DeleteLogsView()
        {

            var DeleteLogs = _deleteLogs.GetAllDeleteLogs();

            return View(DeleteLogs);
        }



        public async Task<JsonResult> DeletelogListDataTable()
        {
            var dataTable_ = new DataTable_Dto
            {
                Draw = Request.Form["draw"].FirstOrDefault(),
                sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault(),
                sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault(),
                SearchValue = Request.Form["search[value]"].FirstOrDefault(),
                PageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0"),
                Skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0")
            };

            List<DeleteLog_Dto> Records = _deleteLogs.ViewDeleteLogsDataTable(dataTable_);
            var totalRecord = Records.Count();
            var filterRecord = Records.Count();
            var FellowshipList = Records.Skip(dataTable_.Skip).Take(dataTable_.PageSize).ToList();
            var returnObj = new
            {
                draw = dataTable_.Draw,
                recordsTotal = totalRecord,
                recordsFiltered = filterRecord,
                data = FellowshipList
            };
            return Json(returnObj);

        }






    }


}
