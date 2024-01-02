using Microsoft.AspNetCore.Mvc;
using Repository_Logic.DeleteLogsRepository.Interface;
using Repository_Logic.Dto;
using Repository_Logic.ErrorLogsRepository.Interface;
using Repository_Logic.LoginLogsDataRepository.Interface;
using Repository_Logic.RegistorLogsRepository.Interface;
using Repository_Logic.TaskAllocation.Implementation;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;

namespace The_GST_1.Controllers
{
    public class Log_InformationController : Controller
    {
        private readonly IErrorLogs _errorLogs;
        private readonly IRegisterLogs _registerLogs;
        private readonly ILoginLogs _loginLogs;
        private readonly IDeleteLogs _deleteLogs;
        public Log_InformationController(IRegisterLogs registerLogs, ILoginLogs loginLogs, IDeleteLogs deleteLogs, IErrorLogs errorLogs)
        {
            _errorLogs = errorLogs;
            _loginLogs = loginLogs;
            _registerLogs = registerLogs;
            _deleteLogs = deleteLogs;
        }

        public IActionResult RegisterLogView()// Shows Register logs details.
        {
            try
            {
               
                var log = _registerLogs.GetAllRegistorLogs().ToList();
                return View(log);
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "AN ERROR OCCURRED WHILE LOADING REGISTER LOGS DETAILS.";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });

            }

        }
        //public IActionResult RegisterLogViewFellowship()
        //{

        //    var log = _registerLogs.GetAllRegistorLogs().ToList();


        //    return View(log);
        //}


        public async Task<JsonResult> ResistorlogListDataTable() // datatable for register logs details.
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


        public IActionResult LoginLogs()// shows login logs details
        {
            try
            {
                var LoginLogs = _loginLogs.GetLoginLogs();

                return View("LoginLogsView", LoginLogs);
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "AN ERROR OCCURRED WHILE LOADING LOGIN LOGS DETAILS.";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });

            }
        }

        public JsonResult LoginLogsDataTable() //datatable for login logs details.
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



        public IActionResult DeleteLogsView() // delete logs which contains data of deleted user.When any user will deleted then that user will add to delete log.
        {
            try
            {
                
                var DeleteLogs = _deleteLogs.GetAllDeleteLogs();

                return View(DeleteLogs);
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "AN ERROR OCCURRED WHILE LOADING DELETE LOGS DETAILS.";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });
            }

        }



        public async Task<JsonResult> DeletelogListDataTable()// datatable for delete logs.
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