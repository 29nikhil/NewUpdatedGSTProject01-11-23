using Data_Access_Layer.Db_Context;
using Microsoft.AspNetCore.Identity;
using Repository_Logic.Dto;
using Repository_Logic.FellowshipRepository.Interface;
using Repository_Logic.UserOtherDatails.Interface;
using Repository_Logic.ViewFilledGSt.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.ViewFilledGSt.Implemenation
{
    public class ViewFilledGst:IViewGSTFilledGst
    {

        private readonly IExtraDetails _extraDetails;
        private readonly Application_Db_Context _context;
        private readonly IFellowshipRepository _repository;
        private Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;


        public ViewFilledGst(IExtraDetails extraDetails, Application_Db_Context context, IFellowshipRepository repository, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> identityUserManager)
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

        //public async Task<List<ExportExcelSheetData_Dto>> GetReturnedFilesDataForUser(string UserID)
        //{
        //    List<ExportExcelSheetData_Dto> UserData = (
        //                                                           (from t1 in _context.appUser
        //                                                            join t2 in _context.UserDetails on t1.Id equals t2.UserId
        //                                                            join t3 in _context.ExcelSheet on t2.UserId equals t3.UserID
        //                                                            where t3.status == "File Returned" && t3.UserID == UserID
        //                                                            select new ExportExcelSheetData_Dto
        //                                                            {
        //                                                                userId = t1.Id,
        //                                                                UniqueFileId = t3.UniqueFileId,
        //                                                                UserName = t1.UserName,
        //                                                                GSTType = t3.GSTType,
        //                                                                GSTNo = t2.GSTNo,
        //                                                                Email = t1.Email,
        //                                                                OrganisationType = t2.BusinessType,
        //                                                                SessionID = t3.SessionID,
        //                                                                Status = t3.status,
        //                                                                Date = t3.Date,
        //                                                                Year = t3.Date.Year,
        //                                                                Month = t3.Date.Month,


        //                                                            }).Distinct().ToList()
        //                                                         );

        //    var ReturnedFilesData = UserData.Select(item => new ExportExcelSheetData_Dto
        //    {
        //        userId = item.userId,
        //        UniqueFileId = item.UniqueFileId,
        //        UserName = item.UserName,
        //        GSTType = item.GSTType,
        //        GSTNo = item.GSTNo,
        //        Email = item.Email,
        //        OrganisationType = item.OrganisationType,
        //        FileName = FileName(item.UniqueFileId),
        //        SessionID = _repository.GetFellowshisession(item.SessionID).Result,
        //        Status = item.Status,
        //        Date = item.Date,
        //        Year = item.Year,
        //        Month = item.Month,
        //    }).DistinctBy(x => x.UniqueFileId).ToList();

        //    return ReturnedFilesData;
        //}
        //public async Task<List<ExportExcelSheetData_Dto>> GetReturnedFilesDataForUser(string UserID)

         public async Task<List<ViewFilleGSt_Dto>> GetReturnedFilesDataForUser(string UserID)
        {
              List<ViewFilleGSt_Dto> UserData = (
                from t1 in _context.appUser
             join t2 in _context.UserDetails on t1.Id equals t2.UserId
             join t3 in _context.ExcelSheet on t2.UserId equals t3.UserID
             where t3.status == "File Returned" && t3.UserID == UserID
             select new ViewFilleGSt_Dto
      {
         UserID = t1.Id,
         FileID = t3.UniqueFileId,
         GSTType = t3.GSTType,
         GSTNo = t2.GSTNo,
         TaxPeriod="Apr-March",
         BusinessType = t2.BusinessType,
         SessionID = t3.SessionID,
         Status = t3.status,
         Date = t3.Date,
         Year = t3.Date.Year,
         Month = t3.Date.Month,
     }).Distinct().ToList();

            var ReturnedFilesData = UserData.Select(item => new ViewFilleGSt_Dto
            {
                UserID = item.UserID,
                FileID = item.FileID,
                GSTType = item.GSTType,
                GSTNo = item.GSTNo,
                TaxPeriod = item.TaxPeriod,
                BusinessType = item.BusinessType,
                FileName = FileName(item.FileID),
                FiledBy = _repository.GetFellowshisession(item.SessionID).Result,
                Status = item.Status,
                SessionID=item.SessionID,
                CreatedDate = item.Date.Date.ToString(),
                Year = item.Year,
                Month = item.Month,
                
            }).DistinctBy(x => x.FileID).ToList();

            return ReturnedFilesData;

        }

        public async Task<List<ViewFilleGSt_Dto>> ViewReturnFilesDataTableUser(DataTable_Dto dataTable, string UserId)
        {
            List<ViewFilleGSt_Dto> ReturnFileData = new List<ViewFilleGSt_Dto>();
            var user = await _IdentityUserManager.FindByIdAsync(UserId);
            var isInRole = await _IdentityUserManager.IsInRoleAsync(user, "User");

            if (isInRole)
            {
                ReturnFileData = await GetReturnedFilesDataForUser(UserId);


            }
            else
            {
            }

            if (!string.IsNullOrEmpty(dataTable.SearchValue))
            {
                string searchValue = dataTable.SearchValue.ToLower();
                ReturnFileData = ReturnFileData.Where(x =>
                    x.GSTType.ToLower().Contains(searchValue) ||
                    x.GSTNo.ToLower().Contains(searchValue) ||
                    x.TaxPeriod.ToLower().Contains(searchValue) ||
                    x.BusinessType.ToLower().Contains(searchValue) ||
                    x.Year.ToString().Contains(searchValue) ||
                    x.FiledBy.ToString().Contains(searchValue) ||
                    x.Status.ToString().Contains(searchValue) ||
                    x.Year.ToString().Contains(searchValue) ||
                    x.Month.ToString().Contains(searchValue) ||
                    x.CreatedDate.ToString().Contains(searchValue)

                ).ToList();
            }
            if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDirection))
            {
                switch (dataTable.sortColumn)
                {
                   
                    case "GSTType":

                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.GSTType).ToList() : ReturnFileData.OrderByDescending(u => u.GSTType).ToList();
                        break;
                    case "GSTNo":
                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.GSTNo).ToList() : ReturnFileData.OrderByDescending(u => u.GSTNo).ToList();
                        break;
                    case "TaxPeriod":
                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.TaxPeriod).ToList() : ReturnFileData.OrderByDescending(u => u.TaxPeriod).ToList();
                        break;
                    case "BusinessType":
                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.BusinessType).ToList() : ReturnFileData.OrderByDescending(u => u.BusinessType).ToList();
                        break;

                    case "CreatedDate":
                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.CreatedDate).ToList() : ReturnFileData.OrderByDescending(u => u.CreatedDate).ToList();
                        break;
                    case "FileName":
                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.FileName).ToList() : ReturnFileData.OrderByDescending(u => u.FileName).ToList();
                        break;
                    case "FiledBy":
                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.FiledBy).ToList() : ReturnFileData.OrderByDescending(u => u.FiledBy).ToList();
                        break;
                    case "Status":
                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.Status).ToList() : ReturnFileData.OrderByDescending(u => u.Status).ToList();
                        break;

                }
            }

            return ReturnFileData;
        }
    }
}
