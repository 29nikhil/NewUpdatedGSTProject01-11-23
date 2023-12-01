using Microsoft.AspNetCore.Mvc;
using Repository_Logic.Dto;
using Repository_Logic.QueryRepository.Interface;
using Repository_Logic.TaskAllocation.Implementation;
using System.Security.Claims;

namespace The_GST_1.Controllers
{
    public class QueryController : Controller
    {
        private readonly IQuery _query;

        public QueryController(IQuery query)
        {
            _query = query;

        }

        public async Task<IActionResult> QueryListForUser()
        {
            var LoginSessionID = "null";

            if (User.Identity.IsAuthenticated)
            {
                LoginSessionID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            List<Query_Dto> queries = await _query.GetAllQueries(LoginSessionID);

            return View(queries);
        }

        public async Task<JsonResult> QueryListForUserDatatable()
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


        public IActionResult InsertQuestion(string query)
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
            return RedirectToAction("QueryListForUser");
        }

        public async Task<IActionResult> QueryListForCADashboard()
        {
            var LoginSessionID = "null";

            if (User.Identity.IsAuthenticated)
            {
                LoginSessionID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            List<Query_Dto> queries = await _query.GetAllQueries(LoginSessionID);
            return View(queries);
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
            var LoginSessionID = "null";

            if (User.Identity.IsAuthenticated)
            {
                LoginSessionID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            _query.InsertAnswerToQuestion(query_Dto, LoginSessionID);

            return RedirectToAction("QueryListForUser");
        }




    }
}