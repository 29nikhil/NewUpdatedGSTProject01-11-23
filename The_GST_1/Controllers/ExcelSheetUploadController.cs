using Data_Access_Layer.Db_Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Repository_Logic.Dto;
using Repository_Logic.ExcelSheetUploadRepository.Interface;
using Repository_Logic.ExportExcelSheet.Interface;
using Repository_Logic.FellowshipRepository.Interface;
using Repository_Logic.ModelView;
using Repository_Logic.UserOtherDatails.Interface;
using System.Security.Claims;
using System.Web.Providers.Entities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace The_GST_1.Controllers
{
    public class ExcelSheetUploadController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager;
        private readonly IExtraDetails _extraDetails;
        private readonly Application_Db_Context _context;
        private readonly IExcelSheetUpload ExportData;
        private Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;
        private readonly IFellowshipRepository _fellowship;
        public ExcelSheetUploadController(IExtraDetails extraDetails, Application_Db_Context context, IExcelSheetUpload _ExportData, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager, IFellowshipRepository fellowship)
        {
            _context = context;
            _extraDetails = extraDetails;
            ExportData = _ExportData;
            _IdentityUserManager = userManager;
            _fellowship = fellowship;
        }


        public IActionResult ExportExcelSheets()    //ExcelSheetUpload View
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
            //Provide Confirmed User List Username For DropDownList 
            ViewBag.UserList = new SelectList(userList, "Value", "Text");
            return View();
        }


        [HttpGet]
        public IActionResult GetUserDetails(string ID) //Get Dropdown List Select User Name and Show Partial View For User Details
        {
            var UserData = ExportData.UserOtherDetailsForExport(ID);

            return PartialView("_UserDetailsForExport", UserData);
            //partial View Get Details By Drop down list Click

        }



        
        public async Task<IActionResult> Export(IFormFile file, string SelectedUserID, string GSTType) //Export Excelsheet Record in Database 
        {

            if (file != null)
            {
                var LoginuserId = "null"; // User ID of CA or Fellowship who uploaded  a excel sheet. Currently login CA  or Fellowship.
                if (User.Identity.IsAuthenticated)
                {
                    LoginuserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                }
                var Status = ExportData.ExportExcelSheetData(file, LoginuserId,  GSTType,  SelectedUserID);
                var result = await Status;
                if(result== "Success")
                {
                  TempData["ExcelSheetUpload"] = "Excel File Uploaded successfully !";
                }
                else
                {
                   TempData["Status"] = "Excel File Format is wrong.Failed To Upload Excel File";
                    return RedirectToAction("ExportExcelSheets");
                }
              
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



            List<File_Details_Excel_Dto> UserData = await ExportData.GetUserDataForExcelSheet(LoginSessionID);

            return View(UserData);
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


        public async Task<IActionResult> GetExportExcelSheetRecords() //Show User Excel Sheet Records
        {
            var LoginSessionID = "null";

            if (User.Identity.IsAuthenticated)
            {
                LoginSessionID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            var user = await _IdentityUserManager.FindByIdAsync(LoginSessionID);
            var isInRole = await _IdentityUserManager.IsInRoleAsync(user, "Fellowship");
            var ExcelSheetRecords = ExportData.GetDataByFileID(Request.Query["FileId"]);
            bool IsItTaskListView = false;
            bool IsItReturnFileView = false;


            if (Request.Query["IsItTaskListView"].IsNullOrEmpty()|| Request.Query["IsItReturnFileView"].IsNullOrEmpty())
            {

                IsItTaskListView = false;
                IsItReturnFileView = false;
            }
            else
            {
                IsItReturnFileView = true;
                IsItTaskListView = true;
            }
            if (isInRole)
            {
              
                ViewBag.IsItTaskListView = true;
                ViewBag.IsInRoleFellowship = isInRole;
                ViewBag.IsItReturnFileView = true;
                TaskAllowcated_Dto taskAllowcated_Dto = new TaskAllowcated_Dto();
                taskAllowcated_Dto.FileID = Request.Query["FileId"]; taskAllowcated_Dto.CA_ID = LoginSessionID; taskAllowcated_Dto.userID = Request.Query["UserId"]; taskAllowcated_Dto.AllocatedById = Request.Query["UplodedById"];
                var modelTuple = new Tuple<IEnumerable<FileRecords_Dto>, TaskAllowcated_Dto>(ExcelSheetRecords, taskAllowcated_Dto);

                return View(modelTuple);


            }
            else
            {
                ViewBag.IsItReturnFileView = true;
                ViewBag.IsItTaskListView = IsItTaskListView;
                ViewBag.IsInRoleFellowship = isInRole;
                ViewBag.Remark = TempData["Remark"];
                TaskAllowcated_Dto taskAllowcated_Dto = new TaskAllowcated_Dto();
                taskAllowcated_Dto.FileID = Request.Query["FileId"]; taskAllowcated_Dto.CA_ID = LoginSessionID;taskAllowcated_Dto.userID = Request.Query["UserId"]; taskAllowcated_Dto.AllocatedById = Request.Query["UplodedById"];
                var modelTuple = new Tuple<IEnumerable<FileRecords_Dto>, TaskAllowcated_Dto>(ExcelSheetRecords,taskAllowcated_Dto);

                return View( modelTuple);

            }
       
            
            
        }


        public IActionResult EditExcelSheetRecordView(string Id) // Edit View for editing excel sheet records
        {
            
            var ExcelSheetRecords = ExportData.GetDataByFileID(Id);

            return View(ExcelSheetRecords);
        }



        [HttpPost]
        public IActionResult editExcelSheetRecord(string id, string ProductName, string HSE_SAC_Code, string Qty, string Rate, string Ammount, string Disc, string TaxableValue, string SC_GST_Rate, string SC_GST_Ammount, string Total) // Edit the single record of excelsheet
        {

            FileRecords_Dto ExcelSheetRecord = new FileRecords_Dto();
            ExcelSheetRecord.id = id;
            ExcelSheetRecord.ProductName = ProductName;
            ExcelSheetRecord.HSE_SAC_Code = HSE_SAC_Code;
            ExcelSheetRecord.Qty = Qty;
            ExcelSheetRecord.Rate = Rate;
            ExcelSheetRecord.Ammount = Ammount;
            ExcelSheetRecord.Disc = Disc;

            ExcelSheetRecord.TaxableValue = TaxableValue;

            ExcelSheetRecord.SC_GST_Ammount = SC_GST_Ammount;
            ExcelSheetRecord.SC_GST_Rate = SC_GST_Rate;
            ExcelSheetRecord.Total = Total;


            var UniqueFileID = ExportData.UpdateExcelSheetRecord(ExcelSheetRecord);


            return RedirectToAction("EditExcelSheetRecordView", new { Id = UniqueFileID });
        }


        public IActionResult AddNewExcelSheetRecord(FileRecords_Dto Record)// Add the new record in the excel sheet
        {


            ExportData.InsertNewExcelSheetRecord(Record);

            return RedirectToAction("EditExcelSheetRecordView", new { Id = Record.id });
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

        
    }
}
