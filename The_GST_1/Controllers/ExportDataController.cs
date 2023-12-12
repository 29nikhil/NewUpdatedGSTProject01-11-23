using Data_Access_Layer.Db_Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using Repository_Logic.Dto;
using Repository_Logic.ExportExcelSheet.Interface;
using Repository_Logic.FellowshipRepository.Interface;
using Repository_Logic.ModelView;
using Repository_Logic.UserOtherDatails.Interface;
using System.ComponentModel;
using System.Security.Claims;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace The_GST_1.Controllers
{
    [Authorize]
    [Authorize(Roles = "CA,Fellowship")]


    public class ExportDataController : Controller
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager;
        private readonly IExtraDetails _extraDetails;
        private readonly Application_Db_Context _context;
        private readonly IExportExcelSheet ExportData;
        private Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;
        private readonly IFellowshipRepository _fellowship;
        public ExportDataController(IExtraDetails extraDetails, Application_Db_Context context, IExportExcelSheet _ExportData, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager, IFellowshipRepository fellowship)
        {
            _context = context;
            _extraDetails = extraDetails;
            ExportData = _ExportData;
            _IdentityUserManager = userManager;
            _fellowship = fellowship;
        }



        public IActionResult ExportExcelSheets()
        {
            var usersInRole = _extraDetails.GetAllUser();
            List<UserModelView> UserData = new List<UserModelView>();

            foreach (var user in usersInRole)
            {
                if (user.identityUser.EmailConfirmed == true)
                {
                    UserData.Add(user);
                }
            }
            var userList = UserData.Select(user => new SelectListItem
            {
                Text = user.identityUser.UserName,
                Value = user.identityUser.Id.ToString()
            });

            ViewBag.UserList = new SelectList(userList, "Value", "Text");
            return View();
        }


        [HttpGet]
        public IActionResult GetUserDetails(string ID)
        {
            var UserData = ExportData.UserOtherDetailsForExport(ID);

            return PartialView("_UserDetailsForExport", UserData);
            //partial View Get Details By Drop down list Click

        }

        public async Task<IActionResult> Export(IFormFile file, string SelectedUserID, string GSTType)
        {

            if (file != null)
            {
                var LoginuserId = "null"; // User ID of CA or Fellowship who uploaded  a excel sheet. Currently login CA  or Fellowship.
                if (User.Identity.IsAuthenticated)
                {
                    LoginuserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                }
                var Status = ExportData.ExportExcelSheetData(file, SelectedUserID, GSTType, LoginuserId);

                TempData["ExcelSheetUpload"] = "Data uploaded successfully. Excel Files";

                return RedirectToAction("GetExportExcelsheetData");
            }
            else
            {
                TempData["EmptyData"] = "Please Select File ";
                return RedirectToAction("GetExportExcelsheetData"); // Redirect with a message
            }
        }





        //Action Method Excecute After Add Excel File
        public async Task<IActionResult> GetExportExcelsheetData() //Show Metadata List Excel File Uploaded User Name and Files Name 
        {

            var LoginSessionID = "null";
            if (User.Identity.IsAuthenticated)
            {
                LoginSessionID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            var user = await _IdentityUserManager.FindByIdAsync(LoginSessionID);
            var isInFellowshipRole = await _IdentityUserManager.IsInRoleAsync(user, "Fellowship");
            ViewBag.IsFellowship = isInFellowshipRole;
           


            List<ExportExcelSheetData_Dto> UserData = await ExportData.GetUserDataForExcelSheet(LoginSessionID);

            return View(UserData);
        }


        public async Task<IActionResult> GetExportExcelSheetRecords() //Show User Excel Sheet Records
        {
            var LoginSessionID = "null";

            if (User.Identity.IsAuthenticated)
            {
                LoginSessionID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            var user = await _IdentityUserManager.FindByIdAsync(LoginSessionID);
            var isInRole = await _IdentityUserManager.IsInRoleAsync(user, "Fellowship");

            string UniqueFileId = Request.Query["uniqueFileId"];
            string SessionID = _fellowship.GetIDByUserName(Request.Query["sessionID"]);
            string userID = Request.Query["userId"];
            bool IsItReturnFileView = false;
            bool IsItTaskListView = false;
            if (Request.Query["IsItTaskListView"].IsNullOrEmpty()|| Request.Query["IsItReturnFileView"].IsNullOrEmpty())
            {
                IsItReturnFileView = false;
                IsItTaskListView = false;

            }
            else
            {
                IsItReturnFileView = true;
                IsItTaskListView = true;
            }
            ViewBag.IsItReturnFileView = IsItReturnFileView;
            ViewBag.IsItTaskListView = IsItTaskListView;
            ViewBag.IsInRoleFellowship = isInRole;
            ViewBag.UniqueFileId = UniqueFileId;
            ViewBag.SessionID = SessionID;
            ViewBag.UserID = userID;
            ViewBag.LoginSessionID = LoginSessionID;
            var ExcelSheetRecords = ExportData.GetDataByFileID(UniqueFileId);
            ViewBag.Remark = TempData["Remark"];
            return View(ExcelSheetRecords);
        }






        public JsonResult ExportExcelSheetRecordsDatatable(string id)
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

            var data = ExportData.ExportExcelSheetRecordsDatatable(dataTable_, id);
            var totalRecords = data.Count;
            var filterRecord = data.Count;
            var filteredData = data.Skip(dataTable_.Skip).Take(dataTable_.PageSize).ToList();

            var result = new
            {
                draw = dataTable_.Draw,
                recordsTotal = totalRecords,
                recordsFiltered = filterRecord,
                data = filteredData
            };

            return Json(result);

        }













        public IActionResult EditExcelSheetRecordView(string Id) // Edit View for editing excel sheet records
        {
            var ExcelSheetRecords = ExportData.GetDataByFileID(Id);

            return View(ExcelSheetRecords);
        }


        [HttpPost]
        public IActionResult editExcelSheetRecord(string itemId, string name, string no, string Add, string GSTType, string status, string Date) // Edit the single record of excelsheet
        {

            Excel_Dto ExcelSheetRecord = new Excel_Dto();
            ExcelSheetRecord.id = itemId;
            ExcelSheetRecord.name = name;
            ExcelSheetRecord.no = no;
            ExcelSheetRecord.Add = Add;
            ExcelSheetRecord.GSTType = GSTType;
            ExcelSheetRecord.status = status;

            var UniqueFileID = ExportData.UpdateExcelSheetRecord(ExcelSheetRecord);


            return RedirectToAction("EditExcelSheetRecordView", new { Id = UniqueFileID });
        }


        public IActionResult AddNewExcelSheetRecord(Excel_Dto Record)// Add the new record in the excel sheet
        {


            ExportData.InsertNewExcelSheetRecord(Record);

            return RedirectToAction("EditExcelSheetRecordView", new { Id = Record.UniqueFileId });
        }


        public async Task<JsonResult> ExportExcelSheetDataTable()
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

            var LoginSessionID = "null";
            if (User.Identity.IsAuthenticated)
            {
                LoginSessionID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }


            var data = await ExportData.ExcelSheetDatatable(dataTable_, LoginSessionID);

            // Perform pagination
            var totalRecords = data.Count;
            var filteredData = data.Skip(dataTable_.Skip).Take(dataTable_.PageSize).ToList();

            var result = new
            {
                draw = dataTable_.Draw,
                recordsTotal = totalRecords,
                recordsFiltered = filteredData.Count,
                data = filteredData
            };

            return Json(result);
        }



    }
}
