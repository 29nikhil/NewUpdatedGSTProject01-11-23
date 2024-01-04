using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using OfficeOpenXml;
using Repository_Logic.Dto;
using Repository_Logic.ExcelSheetUploadRepository.Interface;
using Repository_Logic.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

namespace Repository_Logic.ExcelSheetUploadRepository.Implementation
{
    public class ExcelSheetUpload : IExcelSheetUpload
    {

        private readonly Application_Db_Context _context;
        private Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;

        public ExcelSheetUpload(Application_Db_Context context, UserManager<IdentityUser> identityUserManager)
        {
            _context = context;
            _IdentityUserManager = identityUserManager;
        }

        public async Task<string> ExportExcelSheetData(IFormFile file, string UplodedById, string GSTType, string userId)
        {
            File_Details_Excel file_Details_Excel = new File_Details_Excel();
            string Status = "";
            try
            {
                Status = "Success";

                
  
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var list = new List<FileRecords_Dto>();

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        if (!IsValidHeader( worksheet.Cells[3, 2].Text, worksheet.Cells[3, 3].Text,
                                 worksheet.Cells[3, 4].Text, worksheet.Cells[3, 5].Text, worksheet.Cells[3, 6].Text,
                                 worksheet.Cells[3, 7].Text, worksheet.Cells[3, 8].Text, worksheet.Cells[3, 9].Text,
                                 worksheet.Cells[3, 10].Text, worksheet.Cells[3, 11].Text))
                        {
                            Status = "Fail";
                            return Status;
                        }




                        var rowcount = worksheet.Dimension.Rows;
                        for (int row = 4; row <= rowcount; row++)
                        {
                            FileRecords_Dto record = new FileRecords_Dto();
                            record.ProductName = worksheet.Cells[row, 2].Value.ToString().Trim();
                            record.HSE_SAC_Code = worksheet.Cells[row, 3].Value.ToString().Trim();
                            record.Qty = worksheet.Cells[row, 4].Value.ToString().Trim();
                            record.Rate = worksheet.Cells[row, 5].Value.ToString().Trim();
                            record.Ammount = worksheet.Cells[row, 6].Value.ToString().Trim();
                            record.Disc = worksheet.Cells[row, 7].Value.ToString().Trim(); // Total Sale
                            record.TaxableValue = worksheet.Cells[row, 8].Value.ToString().Trim(); //Discount
                                                                                                   // int a = worksheet.Cells[row, 9].GetValue<int>();
                                                                                                   // record.SC_GST_Rate =(a*a*a).ToString();
                            record.SC_GST_Rate = worksheet.Cells[row, 9].Value?.ToString().Substring(2,record.Rate.Length);


                            record.SC_GST_Ammount = worksheet.Cells[row, 10].Value.ToString().Trim();
                            record.Total = worksheet.Cells[row, 11].Value.ToString().Trim();
                            record.FileId = uniqueFileName;
                            record.Date = DateTime.Now;
                            list.Add(record);
                        }
                        
                        // Save the data to the database
                        foreach (var record in list)
                        {
                            Insert(record);
                        }
                        var CAId = _context.userResistorLogs.Where(x=>x.UserID == userId).FirstOrDefault();
                        file_Details_Excel.FileId = uniqueFileName;
                        file_Details_Excel.UserId = userId;
                        file_Details_Excel.GSTTye = GSTType;
                        file_Details_Excel.UplodedById = UplodedById;
                        file_Details_Excel.CA_ID = FindCAId(userId);
                        file_Details_Excel.Status = "File Inserted";
                        file_Details_Excel.Date = DateTime.Now;
                        _context.Add(file_Details_Excel);
                        _context.SaveChanges();
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

        private bool IsValidHeader(params string[] headers)
        {
            
            return headers.SequenceEqual(new[] { "Product Description", "HSN Code", "Qty", "Rate", "Ammount", "Discount", "Taxable Value", "Rate", "Amount", "Total" });
        }


        string FindCAId(string UserID)
        {
            UserRegisterLogs userRegisterLogs = _context.userResistorLogs.Where(x => x.UserID == UserID).FirstOrDefault();
               

            if (userRegisterLogs != null)
            {
                return userRegisterLogs.CA_ID.ToString();
            }

            // Return a default value or handle the case where no matching record is found.
            return string.Empty; // Or another appropriate default value.
        }
        
        //Insert Method Call save Data in Database Row Wise one by one
        public void Insert(FileRecords_Dto ExcelData)
        {
            FilesRecords record = new FilesRecords();
            record.ProductName = ExcelData.ProductName;
            record.HSE_SAC_Code = ExcelData.HSE_SAC_Code;
            record.Qty = ExcelData.Qty;
            record.Rate = ExcelData.Rate;
            record.NetAmmount = ExcelData.NetAmmount;
            record.Disc = ExcelData.Disc;
            record.TaxableValue = ExcelData.TaxableValue;
            record.SC_GST_Rate = ExcelData.SC_GST_Rate;
            record.SC_GST_Ammount = ExcelData.SC_GST_Ammount;
            record.Total = ExcelData.Total;
            record.FileId = ExcelData.FileId;
            record.Date = ExcelData.Date;
            record.Ammount= ExcelData.Ammount;
           
      //ID of person who uploaded Excel sheet i.e.CA or Fellowship 
            _context.FilesRecords.Add(record);
            _context.SaveChanges();
        }

        public async Task<List<File_Details_Excel_Dto>> GetUserDataForExcelSheet(string LoginSessionID)
        {
            var user = await _IdentityUserManager.FindByIdAsync(LoginSessionID);
            var isInRole = await _IdentityUserManager.IsInRoleAsync(user, "Fellowship");
            List<File_Details_Excel_Dto> UserData = new List<File_Details_Excel_Dto>();

            if (isInRole)
            {
                
               
                UserData = (
                  from T1 in _context.appUser
                  join T2 in _context.UserDetails on T1.Id equals T2.UserId
                  join T3 in _context.File_Details_Excel on T2.UserId equals T3.UserId
                  join T4 in _context.userResistorLogs on T3.UserId equals T4.UserID
                  where (T3.Status == "Changes Pending" || T3.Status == "File Inserted" || T3.Status == "Changes Done") && T3.UplodedById == LoginSessionID


                  select new File_Details_Excel_Dto
                  {
                      FileId = T3.FileId,
                      FileName = FileName(T3.FileId),
                      Status = T3.Status,
                      GSTType = T3.GSTTye,
                      GstNo = T2.GSTNo,
                      BusinessType = T2.BusinessType,
                      UplodedById = T3.UplodedById,

                      UserId = T2.UserId,
                      UserName = T1.UserName,
                      Email = T1.Email,
                      CA_ID = T4.CA_ID,
                      UplodedByName = _IdentityUserManager.Users.Where(x => x.Id == T3.UplodedById).Select(x => x.UserName).FirstOrDefault(),


                  }).ToList();



            }
            else
            {
                UserData = (
                    from T1 in _context.appUser 
                    join T2 in _context.UserDetails on T1.Id equals T2.UserId
                    join T3 in _context.File_Details_Excel on T2.UserId equals T3.UserId
                    join T4 in _context.userResistorLogs on T3.UserId equals T4.UserID
                    where (T3.Status == "Changes Pending" || T3.Status == "File Inserted" || T3.Status == "Changes Done") 


                    select new File_Details_Excel_Dto
                            {
                                FileId = T3.FileId,
                                FileName = FileName(T3.FileId),
                                Status = T3.Status,
                                GSTType = T3.GSTTye,
                                GstNo = T2.GSTNo,
                                BusinessType = T2.BusinessType,
                                UplodedById = T3.UplodedById,

                                UserId = T2.UserId,
                                UserName = T1.UserName,
                                Email = T1.Email,
                                CA_ID = T4.CA_ID,
                                UplodedByName = _IdentityUserManager.Users.Where(x => x.Id == T3.UplodedById).Select(x => x.UserName).FirstOrDefault(),


                             }).ToList();




            }


            return UserData;
        }


        public static string FileName(string Filename)
        {
            string[] parts = Filename.Split('_');
            string filename = parts.Length >= 2 ? parts[1] : Filename;

            return filename;
        }

       //Show ExcelSheet Files Name And Details
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

        public async Task<List<File_Details_Excel_Dto>> ExcelSheetDatatable(DataTable_Dto dataTable, string LoginSessionID)
        {
            var UserData = await GetUserDataForExcelSheet(LoginSessionID);

            // Filter the data based on the search value
            
            if (!string.IsNullOrEmpty(dataTable.SearchValue))
            {
                string searchValue = dataTable.SearchValue.ToLower();
                UserData = UserData.Where(x =>
                    x.UserName.ToLower().Contains(searchValue) ||
                    x.GSTType.ToLower().Contains(searchValue) ||
                    x.GstNo.ToLower().Contains(searchValue) ||
                    x.FileName.ToLower().Contains(searchValue) ||
                    x.BusinessType.ToLower().Contains(searchValue) ||
                    x.Email.ToLower().Contains(searchValue) ||
                     x.UplodedByName.ToLower().Contains(searchValue) ||

                    x.Status.ToLower().Contains(searchValue)
                ).ToList();
            }

            // Sorting
            if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDirection))
            {
                switch (dataTable.sortColumn)
                {
                    case "UserId":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.UserId).ToList() : UserData.OrderByDescending(u => u.UserId).ToList();
                        break;
                    case "UserName":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.UserName).ToList() : UserData.OrderByDescending(u => u.UserName).ToList();
                        break;
                    case "GSTType":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.GSTType).ToList() : UserData.OrderByDescending(u => u.GSTType).ToList();
                        break;
                    case "GstNo":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.GstNo).ToList() : UserData.OrderByDescending(u => u.GstNo).ToList();
                        break;
                    case "Email":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.Email).ToList() : UserData.OrderByDescending(u => u.Email).ToList();
                        break;
                    case "BusinessType":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.BusinessType).ToList() : UserData.OrderByDescending(u => u.BusinessType).ToList();
                        break;
                    case "FileName":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.FileName).ToList() : UserData.OrderByDescending(u => u.FileName).ToList();
                        break;
                    case "FileId":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.FileId).ToList() : UserData.OrderByDescending(u => u.FileId).ToList();
                        break;
                    case "SessionID":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.UplodedById).ToList() : UserData.OrderByDescending(u => u.UplodedById).ToList();
                        break;
                    case "Status":
                        UserData = dataTable.sortColumnDirection == "asc" ? UserData.OrderBy(u => u.Status).ToList() : UserData.OrderByDescending(u => u.Status).ToList();
                        break;

                }
            }

            return UserData;
        }

        public List<FileRecords_Dto> GetDataByFileID(string FileID)
        {

            var ExcelSheetRecords = _context.FilesRecords.Where(p => p.FileId == FileID).OrderByDescending(p => p.Date).Select(
                           p => new FileRecords_Dto
                           {

                               id = p.id,
                               ProductName = p.ProductName,
                               HSE_SAC_Code = p.HSE_SAC_Code,
                               Qty = p.Qty,
                               Rate = p.Rate,
                               Ammount = p.Ammount,
                               Disc = p.Disc,
                               TaxableValue = p.TaxableValue,
                               SC_GST_Rate = p.SC_GST_Rate ,
                               SC_GST_Ammount = p.SC_GST_Ammount,
                               Total = p.Total,
                               NetAmmount = p.NetAmmount,
                               Date = p.Date,
                               FileId = p.FileId,
                           }

                        ).ToList();
            return ExcelSheetRecords;
        }

        public List<FileRecords_Dto> ExportExcelSheetRecordsDatatable(DataTable_Dto dataTable, string id)
        {
            var ExcelSheetRecords = GetDataByFileID(id);

            if (!string.IsNullOrEmpty(dataTable.SearchValue))
            {
                string searchValue = dataTable.SearchValue.ToLower();
                ExcelSheetRecords = ExcelSheetRecords.Where(x =>
                    x.ProductName.ToLower().Contains(searchValue) ||
                    x.HSE_SAC_Code.ToLower().Contains(searchValue) ||
                    x.Qty.ToLower().Contains(searchValue) ||
                    x.Rate.ToLower().Contains(searchValue) ||
                    x.Ammount.ToLower().Contains(searchValue) ||
                    x.Disc.ToLower().Contains(searchValue) ||
                    x.TaxableValue.ToLower().Contains(searchValue) ||
                    x.SC_GST_Rate.ToLower().Contains(searchValue) ||
                    x.SC_GST_Ammount.ToLower().Contains(searchValue) ||
                    x.Total.ToLower().Contains(searchValue) 


                ).ToList();
            }


            if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDirection))
            {
                switch (dataTable.sortColumn)
                {
                    case "ProductName":
                        ExcelSheetRecords = dataTable.sortColumnDirection == "asc" ? ExcelSheetRecords.OrderBy(u => u.ProductName).ToList() : ExcelSheetRecords.OrderByDescending(u => u.ProductName).ToList();
                        break;
                    case "HSE_SAC_Code":
                        ExcelSheetRecords = dataTable.sortColumnDirection == "asc" ? ExcelSheetRecords.OrderBy(u => u.HSE_SAC_Code).ToList() : ExcelSheetRecords.OrderByDescending(u => u.HSE_SAC_Code).ToList();
                        break;
                    case "Qty":
                        ExcelSheetRecords = dataTable.sortColumnDirection == "asc" ? ExcelSheetRecords.OrderBy(u => u.Qty).ToList() : ExcelSheetRecords.OrderByDescending(u => u.Qty).ToList();
                        break;
                    case "Rate":
                        ExcelSheetRecords = dataTable.sortColumnDirection == "asc" ? ExcelSheetRecords.OrderBy(u => u.Rate).ToList() : ExcelSheetRecords.OrderByDescending(u => u.Rate).ToList();
                        break;
                    case "Ammount":
                        ExcelSheetRecords = dataTable.sortColumnDirection == "asc" ? ExcelSheetRecords.OrderBy(u => u.Ammount).ToList() : ExcelSheetRecords.OrderByDescending(u => u.Ammount).ToList();
                        break;
                    case "Disc":
                        ExcelSheetRecords = dataTable.sortColumnDirection == "asc" ? ExcelSheetRecords.OrderBy(u => u.Disc).ToList() : ExcelSheetRecords.OrderByDescending(u => u.Disc).ToList();
                        break;
                    case "TaxableValue":
                        ExcelSheetRecords = dataTable.sortColumnDirection == "asc" ? ExcelSheetRecords.OrderBy(u => u.TaxableValue).ToList() : ExcelSheetRecords.OrderByDescending(u => u.TaxableValue).ToList();
                        break;
                    case "SC_GST_Rate":
                        ExcelSheetRecords = dataTable.sortColumnDirection == "asc" ? ExcelSheetRecords.OrderBy(u => u.SC_GST_Rate).ToList() : ExcelSheetRecords.OrderByDescending(u => u.SC_GST_Rate).ToList();
                        break;
                    case "SC_GST_Ammount":
                        ExcelSheetRecords = dataTable.sortColumnDirection == "asc" ? ExcelSheetRecords.OrderBy(u => u.SC_GST_Ammount).ToList() : ExcelSheetRecords.OrderByDescending(u => u.SC_GST_Ammount).ToList();
                        break;
                  
                    case "Total":
                        ExcelSheetRecords = dataTable.sortColumnDirection == "asc" ? ExcelSheetRecords.OrderBy(u => u.Total).ToList() : ExcelSheetRecords.OrderByDescending(u => u.Total).ToList();
                        break;
                    case "Date":
                        ExcelSheetRecords = dataTable.sortColumnDirection == "asc" ? ExcelSheetRecords.OrderBy(u => u.Date).ToList() : ExcelSheetRecords.OrderByDescending(u => u.Date).ToList();
                        break;
                   
                }
            }



            return ExcelSheetRecords;
        }


        public void UpdateStatus(string FileID, string StatusToBeUpdate)
        {

          
                try
                {

                var record=_context.File_Details_Excel.Where(x=>x.FileId == FileID).FirstOrDefault();
                    record.Status = StatusToBeUpdate;
                    _context.Entry(record).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                catch
                {

                }

           
        }


        //Edit ExcelSheet Records

        public string UpdateExcelSheetRecord(FileRecords_Dto ExcelData)
        {
            FilesRecords record = _context.FilesRecords.Where(p => p.id == ExcelData.id).FirstOrDefault();
            record.ProductName = ExcelData.ProductName;
            record.HSE_SAC_Code = ExcelData.HSE_SAC_Code;
            record.Qty = ExcelData.Qty;
            record.Rate = ExcelData.Rate;
            record.Total = ExcelData.Total;
            record.Disc = ExcelData.Disc;
            record.TaxableValue = ExcelData.TaxableValue;
            record.SC_GST_Rate = ExcelData.SC_GST_Rate;
            record.SC_GST_Ammount = ExcelData.SC_GST_Ammount;
            record.Total = ExcelData.Total;
            record.Date = ExcelData.Date;
            record.Ammount = ExcelData.Ammount;

            _context.Entry(record).State = EntityState.Modified;
            _context.SaveChanges();

            return record.FileId;
        }


        //New Excle Sheet Record Add One row 
        public void InsertNewExcelSheetRecord(FileRecords_Dto ExcelData)
        {

            FilesRecords record = new FilesRecords();
            record.ProductName = ExcelData.ProductName;
            record.HSE_SAC_Code = ExcelData.HSE_SAC_Code;
            record.Qty = ExcelData.Qty;
            record.Rate = ExcelData.Rate;
            record.Total = ExcelData.Total;
            record.Disc = ExcelData.Disc;
            record.TaxableValue = ExcelData.TaxableValue;
            record.SC_GST_Rate = ExcelData.SC_GST_Rate;
            record.SC_GST_Ammount = ExcelData.SC_GST_Ammount;
            record.Total = ExcelData.Total;
            record.FileId = ExcelData.FileId;
            record.Date = ExcelData.Date;
            record.Ammount = ExcelData.Ammount;

            //ID of person who uploaded Excel sheet i.e.CA or Fellowship 
            _context.FilesRecords.Add(record);
            _context.SaveChanges();
        }

        
    }
}
