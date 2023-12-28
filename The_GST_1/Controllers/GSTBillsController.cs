using Data_Access_Layer.Db_Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository_Logic.Dto;
using Repository_Logic.ErrorLogsRepository.Interface;
using Repository_Logic.ExcelSheetUploadRepository.Implementation;
using Repository_Logic.ExcelSheetUploadRepository.Interface;
using Repository_Logic.ExportExcelSheet.Implemantation;
using Repository_Logic.ExportExcelSheet.Interface;
using Repository_Logic.GstBill.Interface;
using Repository_Logic.UserOtherDatails.Interface;
using System.Security.Claims;

namespace The_GST_1.Controllers
{
    [Authorize(Roles = "CA,Fellowship")]
    public class GSTBillsController : Controller
    {
        private readonly Application_Db_Context _context;
        private Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;
        private readonly IExtraDetails _extraDetails;
        private readonly IExcelSheetUpload ExportData;
        private readonly IGSTBills _gstbills;
        private readonly IErrorLogs _errorLogs;
        public GSTBillsController(IExtraDetails extraDetails, IExcelSheetUpload _ExportData, Application_Db_Context context, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> IdentityUserManager, IGSTBills gstbills, IErrorLogs errorLogs)
        {
            _errorLogs = errorLogs;
            _IdentityUserManager = IdentityUserManager;
            _context = context;
            ExportData = _ExportData;
            _extraDetails = extraDetails;
            _gstbills = gstbills;
        }

        public IActionResult GSTBillsView()
        {
            try
            {
                
                var userId = Request.Query["userId"];
                var date = Request.Query["date"];
                var Email = Request.Query["email"];
                var fileName = Request.Query["fileName"];
                var gstNo = Request.Query["gstNo"];
                var gstType = Request.Query["gstType"];
                var organisationType = Request.Query["businessType"];
                var sessionID = Request.Query["uplodedById"];
                var UplodedByName = Request.Query["uplodedByName"];

                var status = Request.Query["status"];
                var uniqueFileId = Request.Query["fileId"];
                var userName = Request.Query["username"];



                File_Details_Excel_Dto exportExcelSheetData_Dto = new File_Details_Excel_Dto();
                exportExcelSheetData_Dto.UserId = userId;
                exportExcelSheetData_Dto.Email = Email;
                exportExcelSheetData_Dto.FileName = fileName;
                exportExcelSheetData_Dto.GstNo = gstNo;
                exportExcelSheetData_Dto.GSTType = gstType;
                exportExcelSheetData_Dto.BusinessType = organisationType;
                exportExcelSheetData_Dto.UplodedById = sessionID;
                exportExcelSheetData_Dto.Status = status;
                exportExcelSheetData_Dto.FileId = uniqueFileId;
                exportExcelSheetData_Dto.UserName = userName;
                exportExcelSheetData_Dto.UplodedByName = UplodedByName;


                return View(exportExcelSheetData_Dto);
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                var errorMessage = "AN ERROR OCCURRED WHILE DISPLAYING GST BILLS VIEW.";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });
            }

        }


        public IActionResult InsertGSTBillData(GSTBills_Dto gstBillsData)
        {
            try
            {
               
                var LoginSessionID = "null";

                if (User.Identity.IsAuthenticated)
                {
                    LoginSessionID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                }

                _gstbills.InsertGSTBillsDetails(gstBillsData, LoginSessionID);
                var FileDetails = ExportData.GetDataByFileID(gstBillsData.FileID);
                if (FileDetails != null)
                {
                    var StatusToBeUpdate = "File Returned and GST Bill Submitted";
                    ExportData.UpdateStatus(gstBillsData.FileID, StatusToBeUpdate);

                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);
                TempData["ErrorMessage"] = "AN ERROR OCCURRED WHILE SUBMITTING THE GST BILLS. ";

                return Json(new { success = false, message = TempData["ErrorMessage"] });

            }
        }



        public IActionResult ShowGSTBills()
        {

            try
            {
                
                var GSTBills = _gstbills.GetGSTBillsDetails();


                return View(GSTBills);
            }
            catch (Exception ex)
            {
                ErrorLog_Dto errorLog_Dto = new ErrorLog_Dto();

                errorLog_Dto.Date = DateTime.Now;
                errorLog_Dto.Message = ex.Message;
                errorLog_Dto.StackTrace = ex.StackTrace;

                _errorLogs.InsertErrorLog(errorLog_Dto);

                var errorMessage = "AN ERROR OCCURRED WHILE DISPLAYING GST BILLS.";
                return RedirectToAction("ErrorHandling", "Home", new { ErrorMessage = errorMessage });


            }
        }


        public async Task<JsonResult> ShowGSTBillsDatatable()
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

            List<GSTBills_Dto> Records = _gstbills.ShowGSTBillsDatatable(dataTable_, LoginSessionID);
            var totalRecord = Records.Count();
            var filterRecord = Records.Count();
            var GSTBillsList = Records.Skip(dataTable_.Skip).Take(dataTable_.PageSize).ToList();
            var returnObj = new
            {
                draw = dataTable_.Draw,
                recordsTotal = totalRecord,
                recordsFiltered = filterRecord,
                data = GSTBillsList
            };
            return Json(returnObj);

        }



    }
}