﻿using Microsoft.AspNetCore.Mvc;


    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Repository_Logic.Dto;
    using Repository_Logic.ExportExcelSheet.Interface;
    using Repository_Logic.ReturnFilesRepository.Interface;
    using System.Security.Claims;
using Repository_Logic.ExcelSheetUploadRepository.Interface;

    namespace The_GST_1.Controllers
    {
        public class ReturnFilesRecordsController : Controller
        {
            private readonly IExcelSheetUpload _exportExcelSheet;
            private readonly IReturnFilesRepository _ReturnFile;
            private Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;

            public ReturnFilesRecordsController(IExcelSheetUpload exportExcelSheet, IReturnFilesRepository returnFile, UserManager<IdentityUser> identityUserManager)
            {
                _exportExcelSheet = exportExcelSheet;
                _ReturnFile = returnFile;
                _IdentityUserManager = identityUserManager;
            }



            public IActionResult ReturnFile(string Id)
            {
                var FileDetails = _exportExcelSheet.GetDataByFileID(Id);
                var StatusToBeUpdate = "File Returned";
                if (FileDetails != null)
                {
                _exportExcelSheet.UpdateStatus(Id, StatusToBeUpdate);
                return RedirectToAction("GetExportExcelsheetData", "ExcelSheetUpload");

            }
            return RedirectToAction("GetExportExcelsheetData", "ExcelSheetUpload");

        }


        public async Task<IActionResult> ViewReturnFilesData()
            {
                var LoginSessionID = "null";
                List<File_Details_Excel_Dto> ReturnFileData = new List<File_Details_Excel_Dto>();
                if (User.Identity.IsAuthenticated)
                {
                    LoginSessionID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                }
                var user = await _IdentityUserManager.FindByIdAsync(LoginSessionID);
                var isInRole = await _IdentityUserManager.IsInRoleAsync(user, "Fellowship");
                if (isInRole)
                {
                    ReturnFileData = await _ReturnFile.GetReturnedFilesDataForFellowship(LoginSessionID);

                }
                else
                {
                    ReturnFileData = await _ReturnFile.GetReturnedFilesData();
                }
                return View(ReturnFileData);
            }




            public async Task<JsonResult> ViewReturnFilesDataDataTable()
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
                var data = await _ReturnFile.ViewReturnFilesDataTable(dataTable_, LoginSessionID);

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

            public IActionResult Index()
            {
                return View();
            }
        }
    }
