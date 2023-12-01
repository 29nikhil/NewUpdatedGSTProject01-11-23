using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Repository_Logic.Dto;
using Repository_Logic.ExportExcelSheet.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.ExportExcelSheet.Implemantation
{
    public class ExportExcelSheet : IExportExcelSheet
    {
        private readonly Application_Db_Context _context;
        private Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;
        private readonly IExportExcelSheet ExportData;
        public ExportExcelSheet(Application_Db_Context context, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> IdentityUserManager)
        {
            _IdentityUserManager = IdentityUserManager;
            _context = context;
        }



        public async Task<string> ExportExcelSheetData(IFormFile file, string hiddenInput, string GSTType, string userId)
        {
            string Status = "";
            try
            {
                Status = "Success";


                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var list = new List<Excel_Dto>();

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowcount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowcount; row++)
                        {
                            Excel_Dto record = new Excel_Dto();
                            record.name = worksheet.Cells[row, 2].Value.ToString().Trim();
                            record.no = worksheet.Cells[row, 3].Value.ToString().Trim();
                            record.Add = worksheet.Cells[row, 4].Value.ToString().Trim();
                            record.UserID = hiddenInput;
                            record.GSTType = GSTType;
                            record.status = "Data Inserted";
                            record.UniqueFileId = uniqueFileName;
                            record.SessionID = userId;
                            record.Date = DateTime.Now;
                            list.Add(record);
                        }

                        // Save the data to the database
                        foreach (var record in list)
                        {
                            Insert(record);
                        }
                    }
                }

                return Status;


            }
            catch (Exception ex)
            {
                Status = "Fail";

                return Status;

            }


        }
        public List<ExcelSheetData> GetDataByFileID(string FileID)
        {

            var ExcelSheetRecords = _context.ExcelSheet.Where(p => p.UniqueFileId == FileID).OrderByDescending(p => p.Date).Select(
                           p => new ExcelSheetData
                           {

                               id = p.id,
                               name = p.name,
                               no = p.no,
                               Add = p.Add,
                               UserID = p.UserID,
                               GSTType = p.GSTType,
                               status = p.status,
                               UniqueFileId = p.UniqueFileId,
                               Date = p.Date,
                               SessionID = p.SessionID,
                               ExtractedDate = p.Date.Date.ToString("yyyy-MM-dd")
                           }

                        ).ToList();
            return ExcelSheetRecords;
        }

        public string UpdateExcelSheetRecord(Excel_Dto ExcelSheetRecord)
        {
            ExcelSheetData Record = _context.ExcelSheet.Where(p => p.id == ExcelSheetRecord.id).FirstOrDefault();
            Record.name = ExcelSheetRecord.name;
            Record.no = ExcelSheetRecord.no;
            Record.Add = ExcelSheetRecord.Add;
            Record.GSTType = ExcelSheetRecord.GSTType;
            Record.status = ExcelSheetRecord.status;

            _context.Entry(Record).State = EntityState.Modified;
            _context.SaveChanges();

            return Record.UniqueFileId;
        }


        public void Insert(Excel_Dto ExcelData)
        {
            ExcelSheetData record = new ExcelSheetData();
            record.no = ExcelData.no;
            record.name = ExcelData.name;
            record.Add = ExcelData.Add;
            record.UserID = ExcelData.UserID;
            record.status = ExcelData.status;
            record.GSTType = ExcelData.GSTType;
            record.UniqueFileId = ExcelData.UniqueFileId;
            record.SessionID = ExcelData.SessionID;  //ID of person who uploaded Excel sheet i.e.CA or Fellowship 
            record.Date = ExcelData.Date;
            _context.ExcelSheet.Add(record);
            _context.SaveChanges();
        }

        public void updateStatusFieldOfFileData(List<ExcelSheetData> excelSheetData, string StatusToBeUpdate)
        {

            foreach (var record in excelSheetData)
            {

                try {


                    record.status = StatusToBeUpdate;
                    _context.Entry(record).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                catch
                {
                    
                }
              
            }
        }

        public void InsertNewExcelSheetRecord(Excel_Dto ExcelSheetRecord)
        {

            ExcelSheetData excelSheetData = new ExcelSheetData();

            excelSheetData.name = ExcelSheetRecord.name;
            excelSheetData.no = ExcelSheetRecord.no;
            excelSheetData.Add = ExcelSheetRecord.Add;
            excelSheetData.UserID = ExcelSheetRecord.UserID;
            excelSheetData.GSTType = ExcelSheetRecord.GSTType;
            excelSheetData.status = ExcelSheetRecord.status;
            excelSheetData.UniqueFileId = ExcelSheetRecord.UniqueFileId;
            excelSheetData.SessionID = ExcelSheetRecord.SessionID; //ID of person who uploaded Excel sheet i.e.CA or Fellowship 
            excelSheetData.Date = DateTime.Now;
            _context.ExcelSheet.Add(excelSheetData);
            _context.SaveChanges();
        }

        public UserOtherDetailsForExport_Dto UserOtherDetailsForExport(string ID)
        {
            UserOtherDetailsForExport_Dto UserData = ((from t1 in _context.appUser
                                                       where t1.Id == ID
                                                       join t2 in _context.UserDetails
                                                       on t1.Id equals t2.UserId
                                                       select new UserOtherDetailsForExport_Dto
                                                       {
                                                           UserName = t1.UserName,
                                                           GST = t2.GSTNo,
                                                           BusinessType = t2.BusinessType,
                                                           Email = t1.Email
                                                       })).FirstOrDefault();


            return UserData;

        }
        public static string FileName(string Filename)
        {
            string[] parts = Filename.Split('_');
            string filename = parts.Length >= 2 ? parts[1] : Filename;

            return filename;
        }

        public async Task<List<ExportExcelSheetData_Dto>> GetUserDataForExcelSheet(string LoginSessionID)
        {

            var UserName = _IdentityUserManager.Users.Where(x => x.Id == LoginSessionID).Select(x => x.UserName).FirstOrDefault();

            var user = await _IdentityUserManager.FindByIdAsync(LoginSessionID);
            var isInRole = await _IdentityUserManager.IsInRoleAsync(user, "Fellowship");
            List<ExportExcelSheetData_Dto> UserData = new List<ExportExcelSheetData_Dto>();
            if (isInRole)
            {
                UserData = (
                             (from t1 in _context.appUser
                              join t2 in _context.UserDetails on t1.Id equals t2.UserId
                              join t3 in _context.ExcelSheet on t2.UserId equals t3.UserID
                              where (t3.status == "Changes Pending" || t3.status == "Data Inserted" || t3.status == "Changes Done") && t3.SessionID == LoginSessionID
                              select new ExportExcelSheetData_Dto
                              {
                                  userId = t1.Id,
                                  UniqueFileId = t3.UniqueFileId,
                                  UserName = t1.UserName,
                                  GSTType = t3.GSTType,
                                  GSTNo = t2.GSTNo,
                                  Email = t1.Email,
                                  OrganisationType = t2.BusinessType,
                                  SessionID = _IdentityUserManager.Users.Where(x => x.Id == t3.SessionID).Select(x => x.UserName).FirstOrDefault(),
                                  FileName = ExportExcelSheet.FileName(t3.UniqueFileId),
                                  Status = t3.status

                              }).Distinct().ToList()
                                         );


            }
            else
            {
                UserData = (
                             (from t1 in _context.appUser
                              join t2 in _context.UserDetails on t1.Id equals t2.UserId
                              join t3 in _context.ExcelSheet on t2.UserId equals t3.UserID
                              where (t3.status == "Changes Pending" || t3.status == "Data Inserted" || t3.status == "Changes Done")
                              select new ExportExcelSheetData_Dto
                              {
                                  userId = t1.Id,
                                  UniqueFileId = t3.UniqueFileId,
                                  UserName = t1.UserName,
                                  GSTType = t3.GSTType,
                                  GSTNo = t2.GSTNo,
                                  Email = t1.Email,
                                  OrganisationType = t2.BusinessType,
                                  SessionID = _IdentityUserManager.Users.Where(x => x.Id == t3.SessionID).Select(x => x.UserName).FirstOrDefault(),
                                  FileName = ExportExcelSheet.FileName(t3.UniqueFileId),
                                  Status = t3.status

                              }).Distinct().ToList()
                          );

            }

            return UserData;
        }

        public async Task<List<ExportExcelSheetData_Dto>> ExcelSheetDatatable(DataTable_Dto dataTable, string LoginSessionID)
        {
            var UserData = await GetUserDataForExcelSheet(LoginSessionID);

            // Filter the data based on the search value
            if (!string.IsNullOrEmpty(dataTable.SearchValue))
            {
                string searchValue = dataTable.SearchValue.ToLower();
                UserData = UserData.Where(x =>
                    x.UserName.ToLower().Contains(searchValue) ||
                    x.GSTType.ToLower().Contains(searchValue) ||
                    x.GSTNo.ToLower().Contains(searchValue) ||
                    x.FileName.ToLower().Contains(searchValue) ||
                    x.OrganisationType.ToLower().Contains(searchValue) ||
                    x.Email.ToLower().Contains(searchValue) ||
                    x.Status.ToLower().Contains(searchValue)
                ).ToList();
            }

            // Sorting
            if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDirection))
            {
                switch (dataTable.sortColumn)
                {
                    case "userId":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.userId).ToList() : UserData.OrderByDescending(u => u.userId).ToList();
                        break;
                    case "UserName":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.UserName).ToList() : UserData.OrderByDescending(u => u.UserName).ToList();
                        break;
                    case "GSTType":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.GSTType).ToList() : UserData.OrderByDescending(u => u.GSTType).ToList();
                        break;
                    case "GSTNo":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.GSTNo).ToList() : UserData.OrderByDescending(u => u.GSTNo).ToList();
                        break;
                    case "Email":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.Email).ToList() : UserData.OrderByDescending(u => u.Email).ToList();
                        break;
                    case "OrganisationType":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.OrganisationType).ToList() : UserData.OrderByDescending(u => u.OrganisationType).ToList();
                        break;
                    case "FileName":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.FileName).ToList() : UserData.OrderByDescending(u => u.FileName).ToList();
                        break;
                    case "UniqueFileId":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.UniqueFileId).ToList() : UserData.OrderByDescending(u => u.UniqueFileId).ToList();
                        break;
                    case "SessionID":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.SessionID).ToList() : UserData.OrderByDescending(u => u.SessionID).ToList();
                        break;
                    case "Status":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.Status).ToList() : UserData.OrderByDescending(u => u.Status).ToList();
                        break;

                }
            }

            return UserData;
        }

        public List<ExcelSheetData> ExportExcelSheetRecordsDatatable(DataTable_Dto dataTable, string id)
        {
            var ExcelSheetRecords = GetDataByFileID(id);

            if (!string.IsNullOrEmpty(dataTable.SearchValue))
            {
                string searchValue = dataTable.SearchValue.ToLower();
                ExcelSheetRecords = ExcelSheetRecords.Where(x =>
                    x.name.ToLower().Contains(searchValue) ||
                    x.no.ToLower().Contains(searchValue) ||
                    x.Add.ToLower().Contains(searchValue) ||
                    x.GSTType.ToLower().Contains(searchValue) ||
                    x.status.ToLower().Contains(searchValue) ||
                    x.ExtractedDate.Contains(searchValue)

                ).ToList();
            }


            if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDirection))
            {
                switch (dataTable.sortColumn)
                {
                    case "name":
                        ExcelSheetRecords = dataTable.sortColumnDirection == "asc" ? ExcelSheetRecords.OrderBy(u => u.name).ToList() : ExcelSheetRecords.OrderByDescending(u => u.name).ToList();
                        break;
                    case "no":
                        ExcelSheetRecords = dataTable.sortColumnDirection == "asc" ? ExcelSheetRecords.OrderBy(u => u.no).ToList() : ExcelSheetRecords.OrderByDescending(u => u.no).ToList();
                        break;
                    case "Add":
                        ExcelSheetRecords = dataTable.sortColumnDirection == "asc" ? ExcelSheetRecords.OrderBy(u => u.Add).ToList() : ExcelSheetRecords.OrderByDescending(u => u.Add).ToList();
                        break;
                    case "GSTType":
                        ExcelSheetRecords = dataTable.sortColumnDirection == "asc" ? ExcelSheetRecords.OrderBy(u => u.GSTType).ToList() : ExcelSheetRecords.OrderByDescending(u => u.GSTType).ToList();
                        break;
                    case "status":
                        ExcelSheetRecords = dataTable.sortColumnDirection == "asc" ? ExcelSheetRecords.OrderBy(u => u.status).ToList() : ExcelSheetRecords.OrderByDescending(u => u.status).ToList();
                        break;
                    case "Date":
                        ExcelSheetRecords = dataTable.sortColumnDirection == "asc" ? ExcelSheetRecords.OrderBy(u => u.ExtractedDate).ToList() : ExcelSheetRecords.OrderByDescending(u => u.ExtractedDate).ToList();
                        break;


                }
            }



            return ExcelSheetRecords;
        }
    }

}

