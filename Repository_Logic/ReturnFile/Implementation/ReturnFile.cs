using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Repository_Logic.Dto;
using Repository_Logic.FellowshipRepository.Interface;
using Repository_Logic.ModelView;
using Repository_Logic.ReturnFile.Interface;
using Repository_Logic.UserOtherDatails.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

namespace Repository_Logic.ReturnFile.Implementation
{
    public class ReturnFile : IReturnFile
    {
        private readonly IExtraDetails _extraDetails;
        private readonly Application_Db_Context _context;
        private readonly IFellowshipRepository _repository;
        private Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;


        public ReturnFile(IExtraDetails extraDetails, Application_Db_Context context, IFellowshipRepository repository, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> identityUserManager)
        {
            _extraDetails = extraDetails;
            _context = context;
            _repository = repository;
            _IdentityUserManager = identityUserManager;
        }
        public string FileName(string Filename)
        {
            string[] parts = Filename.Split('_');
            string filename = parts.Length >= 2 ? parts[1] : Filename;

            return filename;
        }

        public async Task<List<ExportExcelSheetData_Dto>> GetReturnedFilesData()
        {
            List<ExportExcelSheetData_Dto> UserData = (
                                                        (from t1 in _context.appUser
                                                         join t2 in _context.UserDetails on t1.Id equals t2.UserId
                                                         join t3 in _context.ExcelSheet on t2.UserId equals t3.UserID
                                                         where t3.status == "File Returned" || t3.status == "File Returned and GST Bill Submitted"
                                                         select new ExportExcelSheetData_Dto
                                                         {
                                                             userId = t1.Id,
                                                             UniqueFileId = t3.UniqueFileId,
                                                             UserName = t1.UserName,
                                                             GSTType = t3.GSTType,
                                                             GSTNo = t2.GSTNo,
                                                             Email = t1.Email,
                                                             OrganisationType = t2.BusinessType,
                                                             SessionID = t3.SessionID,
                                                             Status = t3.status
                                                         }).Distinct().ToList()
                                                      );

            var ReturnedFilesData = new List<ExportExcelSheetData_Dto>();

            foreach (var item in UserData)
            {
                ExportExcelSheetData_Dto exportExcelSheetData_Dto = new ExportExcelSheetData_Dto();
                exportExcelSheetData_Dto.userId = item.userId;
                exportExcelSheetData_Dto.UniqueFileId = item.UniqueFileId;
                exportExcelSheetData_Dto.UserName = item.UserName;
                exportExcelSheetData_Dto.GSTType = item.GSTType;
                exportExcelSheetData_Dto.GSTNo = item.GSTNo;
                exportExcelSheetData_Dto.Email = item.Email;
                exportExcelSheetData_Dto.OrganisationType = item.OrganisationType;
                exportExcelSheetData_Dto.FileName = FileName(item.UniqueFileId);
                exportExcelSheetData_Dto.SessionID = await _repository.GetFellowshisession(item.SessionID);
                exportExcelSheetData_Dto.Status = item.Status;
                ReturnedFilesData.Add(exportExcelSheetData_Dto);
            }

            return ReturnedFilesData;
        }




        public async Task<List<ExportExcelSheetData_Dto>> GetReturnedFilesDataForFellowship(string UserID)
        {
            List<ExportExcelSheetData_Dto> UserData = (
                                                        (from t1 in _context.appUser
                                                         join t2 in _context.UserDetails on t1.Id equals t2.UserId
                                                         join t3 in _context.ExcelSheet on t2.UserId equals t3.UserID
                                                         where t3.status == "File Returned" && t3.SessionID == UserID
                                                         select new ExportExcelSheetData_Dto
                                                         {
                                                             userId = t1.Id,
                                                             UniqueFileId = t3.UniqueFileId,
                                                             UserName = t1.UserName,
                                                             GSTType = t3.GSTType,
                                                             GSTNo = t2.GSTNo,
                                                             Email = t1.Email,
                                                             OrganisationType = t2.BusinessType,
                                                             SessionID = t3.SessionID,
                                                             Status = t3.status
                                                         }).Distinct().ToList()
                                                      );

            var ReturnedFilesData = new List<ExportExcelSheetData_Dto>();

            foreach (var item in UserData)
            {
                ExportExcelSheetData_Dto exportExcelSheetData_Dto = new ExportExcelSheetData_Dto();
                exportExcelSheetData_Dto.userId = item.userId;
                exportExcelSheetData_Dto.UniqueFileId = item.UniqueFileId;
                exportExcelSheetData_Dto.UserName = item.UserName;
                exportExcelSheetData_Dto.GSTType = item.GSTType;
                exportExcelSheetData_Dto.GSTNo = item.GSTNo;
                exportExcelSheetData_Dto.Email = item.Email;
                exportExcelSheetData_Dto.OrganisationType = item.OrganisationType;
                exportExcelSheetData_Dto.FileName = FileName(item.UniqueFileId);
                exportExcelSheetData_Dto.SessionID = await _repository.GetFellowshisession(item.SessionID);
                exportExcelSheetData_Dto.Status = item.Status;
                ReturnedFilesData.Add(exportExcelSheetData_Dto);
            }

            return ReturnedFilesData;
        }

       
        public async Task<List<ExportExcelSheetData_Dto>> ViewReturnFilesDataTable(DataTable_Dto dataTable, string LoginSessionID)
        {
            List<ExportExcelSheetData_Dto> ReturnFileData = new List<ExportExcelSheetData_Dto>();
            var user = await _IdentityUserManager.FindByIdAsync(LoginSessionID);
            var isInRole = await _IdentityUserManager.IsInRoleAsync(user, "Fellowship");

            if (isInRole)
            {
                ReturnFileData = await GetReturnedFilesDataForFellowship(LoginSessionID);

            }
            else
            {
                ReturnFileData = await GetReturnedFilesData();
            }

            if (!string.IsNullOrEmpty(dataTable.SearchValue))
            {
                string searchValue = dataTable.SearchValue.ToLower();
                ReturnFileData = ReturnFileData.Where(x =>
                    x.UserName.ToLower().Contains(searchValue) ||
                    x.GSTType.ToLower().Contains(searchValue) ||
                    x.GSTNo.ToLower().Contains(searchValue) ||
                    x.Email.ToLower().Contains(searchValue) ||
                    x.OrganisationType.ToLower().Contains(searchValue) ||
                    x.FileName.ToString().Contains(searchValue) ||
                    x.SessionID.ToString().Contains(searchValue) ||
                    x.Status.ToString().Contains(searchValue)
                ).ToList();
            }
            if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDirection))
            {
                switch (dataTable.sortColumn)
                {
                    case "UserName":

                        ReturnFileData = dataTable.sortColumnDirection == "asc" ?
                ReturnFileData.OrderBy(u => u.UserName).ToList() :
                ReturnFileData.OrderByDescending(u => u.UserName).ToList();
                        break;
                    case "GSTType":

                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.GSTType).ToList() : ReturnFileData.OrderByDescending(u => u.GSTType).ToList();
                        break;
                    case "GSTNo":
                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.GSTNo).ToList() : ReturnFileData.OrderByDescending(u => u.GSTNo).ToList();
                        break;
                    case "Email":
                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.Email).ToList() : ReturnFileData.OrderByDescending(u => u.Email).ToList();
                        break;
                    case "OrganisationType":
                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.OrganisationType).ToList() : ReturnFileData.OrderByDescending(u => u.OrganisationType).ToList();
                        break;
                    case "FileName":
                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.FileName).ToList() : ReturnFileData.OrderByDescending(u => u.FileName).ToList();
                        break;
                    case "SessionID":
                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.SessionID).ToList() : ReturnFileData.OrderByDescending(u => u.SessionID).ToList();
                        break;
                    case "Status":
                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.Status).ToList() : ReturnFileData.OrderByDescending(u => u.Status).ToList();
                        break;

                }
            }

            return ReturnFileData;
        }



       
        // return Files Data

        public async Task<List<ExportExcelSheetData_Dto>> GetReturnedFilesDataForUser(string UserID)
        {
            List<ExportExcelSheetData_Dto> UserData = (
                                                                   (from t1 in _context.appUser
                                                                    join t2 in _context.UserDetails on t1.Id equals t2.UserId
                                                                    join t3 in _context.ExcelSheet on t2.UserId equals t3.UserID
                                                                    where t3.status == "File Returned" && t3.UserID == UserID 
                                                                    select new ExportExcelSheetData_Dto
                                                                    {
                                                                        userId = t1.Id,
                                                                        UniqueFileId = t3.UniqueFileId,
                                                                        UserName = t1.UserName,
                                                                        GSTType = t3.GSTType,
                                                                        GSTNo = t2.GSTNo,
                                                                        Email = t1.Email,
                                                                        OrganisationType = t2.BusinessType,
                                                                        SessionID = t3.SessionID,
                                                                        Status = t3.status,
                                                                        Date=t3.Date,
                                                                        Year=t3.Date.Year,
                                                                        Month=t3.Date.Month,
                                                                       
                                                                        
                                                                    }).Distinct().ToList()
                                                                 );

            var ReturnedFilesData = UserData.Select(item => new ExportExcelSheetData_Dto
            {
                userId = item.userId,
                UniqueFileId = item.UniqueFileId,
                UserName = item.UserName,
                GSTType = item.GSTType,
                GSTNo = item.GSTNo,
                Email = item.Email,
                OrganisationType = item.OrganisationType,
                FileName = FileName(item.UniqueFileId),
                SessionID = _repository.GetFellowshisession(item.SessionID).Result,
                Status = item.Status,
                Date = item.Date,
                Year = item.Year,
                Month = item.Month,
            }).DistinctBy(x=>x.UniqueFileId).ToList();

            return ReturnedFilesData;
        }

      
    }


}