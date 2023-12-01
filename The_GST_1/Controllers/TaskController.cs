using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository_Logic.Dto;
using Repository_Logic.ExcelSheetUploadRepository.Interface;
using Repository_Logic.ExportExcelSheet.Interface;
using Repository_Logic.FellowshipRepository.Interface;
using Repository_Logic.TaskAllocation.Interface;
using Repository_Logic.UserOtherDatails.Interface;
using System.Security.Claims;

namespace The_GST_1.Controllers
{
    [Authorize(Roles = "CA,Fellowship")]

    public class TaskController : Controller
    {
        private readonly ITaskAllocation _taskAllocation;
        private readonly IExportExcelSheet _exportExcelSheet;
        private readonly IExtraDetails _extraDetails;
        private readonly IFellowshipRepository _fellowship;
        private readonly IExcelSheetUpload _excelSheetUpload;

        private Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;
        public TaskController(ITaskAllocation taskAllocation, IExportExcelSheet exportExcelSheet, IExtraDetails extraDetails, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager, IFellowshipRepository fellowship, IExcelSheetUpload excelSheetUpload)
        {
            _extraDetails = extraDetails;
            _taskAllocation = taskAllocation;
            _exportExcelSheet = exportExcelSheet;
            _IdentityUserManager = userManager;
            _fellowship = fellowship;
            _excelSheetUpload = excelSheetUpload;
        }
        public IActionResult Insert(string UniqueFileId, string SessionID, string UserID, string LoginSessionID, string Remark)
        {


            AllocatedTask_Dto allocatedTask_Dto = new AllocatedTask_Dto();
            allocatedTask_Dto.SessionID = SessionID;
            allocatedTask_Dto.Login_SessionID = LoginSessionID;
            allocatedTask_Dto.Remark = Remark;
            allocatedTask_Dto.FileID = UniqueFileId;
            allocatedTask_Dto.userID = UserID;
            allocatedTask_Dto.status = "Changes Pending";
            allocatedTask_Dto.Created_date = DateTime.Now;
            _taskAllocation.Insert(allocatedTask_Dto);
            var FileDetails = _exportExcelSheet.GetDataByFileID(UniqueFileId);
            var StatusToBeUpdate = "Changes Pending";
            _exportExcelSheet.updateStatusFieldOfFileData(FileDetails, StatusToBeUpdate);

            return RedirectToAction("GetExportExcelsheetData", "ExportData");
        }

        public IActionResult InsertTask(TaskAllowcated_Dto allocatedTask_Dto)// New Update 0.1
        {

            _taskAllocation.InsertTask(allocatedTask_Dto);
             _excelSheetUpload.UpdateStatus(allocatedTask_Dto.FileID, allocatedTask_Dto.status);



            return RedirectToAction("GetExportExcelsheetData", "ExcelSheetUpload");
        }

        public async Task<IActionResult> TaskListView() //New Update 0.1
        {
            var LoginSessionID = "null";

            if (User.Identity.IsAuthenticated)
            {
                LoginSessionID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

          
        
             var data=  await _taskAllocation.TaskListAsync(LoginSessionID);

           


            return View(data);
        }
        public async Task<IActionResult> ViewTaskList()
        {
            var LoginSessionID = "null";
            var Records = new List<TaskList_Dto>();

            if (User.Identity.IsAuthenticated)
            {
                LoginSessionID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            var user = await _IdentityUserManager.FindByIdAsync(LoginSessionID);
            var isInRole = await _IdentityUserManager.IsInRoleAsync(user, "Fellowship");

            if (isInRole)
            {
                Records = _taskAllocation.GetDataBySessionIDForFellowship(LoginSessionID);

            }
            else
            {

                Records = _taskAllocation.GetDataBySessionID(LoginSessionID);
            }


            return View(Records);
        }




        public async Task<JsonResult> TaskListDataTable() //New Update 0.1
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

            List<TaskView_Dto> Records = await _taskAllocation.ViewTaskListDataTable(dataTable_, LoginSessionID);
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

        public async Task<JsonResult> ViewTaskListDataTable()
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

            List<TaskList_Dto> Records = await _taskAllocation.ViewTaskDataTable(dataTable_, LoginSessionID);
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





        public IActionResult UpdateTheStatusField(string Id)
        {
            
            _taskAllocation.ChangesDoneTask(Id);
            //_taskAllocation.ChangesDone(Id);

            return RedirectToAction("TaskListView");
        }




    }
}

