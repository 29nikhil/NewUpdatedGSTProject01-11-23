using Data_Access_Layer.Db_Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository_Logic.Dto;
using Repository_Logic.ExcelSheetUploadRepository.Implementation;
using Repository_Logic.ExcelSheetUploadRepository.Interface;
using Repository_Logic.ExportExcelSheet.Implemantation;
using Repository_Logic.ExportExcelSheet.Interface;
using Repository_Logic.GstBill.Interface;
using Repository_Logic.UserOtherDatails.Interface;

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

        public GSTBillsController(IExtraDetails extraDetails, IExcelSheetUpload _ExportData, Application_Db_Context context, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> IdentityUserManager, IGSTBills gstbills)
        {
            _IdentityUserManager = IdentityUserManager;
            _context = context;
            ExportData = _ExportData;
            _extraDetails = extraDetails;
            _gstbills = gstbills;
        }

        public IActionResult GSTBillsView()
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


        public IActionResult InsertGSTBillData(GSTBills_Dto gstBillsData)
        {

            
            _gstbills.InsertGSTBillsDetails(gstBillsData);
            var FileDetails = ExportData.GetDataByFileID(gstBillsData.FileID);
            if(FileDetails != null)
            {
                var StatusToBeUpdate = "File Returned and GST Bill Submitted";
                ExportData.UpdateStatus(gstBillsData.FileID, StatusToBeUpdate);

            }

            return RedirectToAction("ViewReturnFilesData", "ReturnFilesRecords");
        }

    }
}
