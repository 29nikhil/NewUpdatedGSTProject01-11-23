using Microsoft.AspNetCore.Mvc;
using Repository_Logic.Dto;
using Repository_Logic.ErrorLogsRepository.Interface;
using Repository_Logic.QueryRepository.Interface;
using Repository_Logic.TaskAllocation.Implementation;
using System.Security.Claims;

namespace The_GST_1.Controllers
{
    public class QueryController : Controller
    {
        private readonly IQuery _query;

        private readonly IErrorLogs _errorLogs;
        public QueryController(IQuery query, IErrorLogs errorLogs)
        {
            _query = query;
            _errorLogs = errorLogs;
        }

        public async Task<IActionResult> QueryListForUser()// List of the Queries submitted by user for user side dashboard.
        {
            try
            {
                
                var LoginSessionID = "null";

                if (User.Identity.IsAuthenticated)
                {
                    LoginSessionID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                }

                List<Query_Dto> queries = await _query.GetAllQueries(LoginSessionID);
                return View(queries);
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "AN ERROR OCCURRED WHILE LOADING QUERIES.";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });
            }

        }

        public async Task<JsonResult> QueryListForUserDatatable()// Datatable for list of the queries provided by user for user side dashboard.
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

            if (User.Identity.IsAuthenticated)
            {
                LoginSessionID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            List<Query_Dto> Records = await _query.QueryListForUserDatatable(dataTable_, LoginSessionID);
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



        public IActionResult InsertQuestion(string query)// Question inserted by the user (User side dashboard).
        {
            try
            {
               
                var LoginSessionID = "null";

                if (User.Identity.IsAuthenticated)
                {
                    LoginSessionID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                }
                Query_Dto query_Dto = new Query_Dto();
                query_Dto.Question = query;
                query_Dto.UserID = LoginSessionID;
                query_Dto.QuestionAskedDate = DateTime.Now;
                _query.insert(query_Dto);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                TempData["ErrorMessage"] = "AN ERROR OCCURRED WHILE SUBMITTING THE QUERY. ";

                return Json(new { success = false, message = TempData["ErrorMessage"] });
            }

        }

        public async Task<IActionResult> QueryListForCADashboard() // list of the queries at CA dashboard.
        {
            try
            {
               
                var LoginSessionID = "null";

                if (User.Identity.IsAuthenticated)
                {
                    LoginSessionID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                }
                List<Query_Dto> queries = await _query.GetAllQueries(LoginSessionID);
                return View(queries);
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "AN ERROR OCCURRED WHILE LOADING QUERIES.";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });

            }
        }


        public async Task<JsonResult> QueryListForCADatatable()
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

            if (User.Identity.IsAuthenticated)
            {
                LoginSessionID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            List<Query_Dto> Records = await _query.QueryListForCADatatable(dataTable_, LoginSessionID);
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


        public IActionResult InsertAnswerToQuestion(Query_Dto query_Dto)
        {

            try
            {
                

                var LoginSessionID = "null";

                if (User.Identity.IsAuthenticated)
                {
                    LoginSessionID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                }

                _query.InsertAnswerToQuestion(query_Dto, LoginSessionID);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "AN ERROR OCCURRED WHILE SUBMITTING THE ANSWER.";
                return Json(new { success = false, message = errorMessage });

            }

        }




    }
}