using Data_Access_Layer.Db_Context;
using Microsoft.AspNetCore.Identity;
using Repository_Logic.Dto;
using Repository_Logic.FellowshipRepository.Interface;
using Repository_Logic.ReturnFilesRepository.Interface;
using Repository_Logic.UserOtherDatails.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.ReturnFilesRepository.Implementation
{
    public class ReturnFilesRepository : IReturnFilesRepository
    {
        private readonly IExtraDetails _extraDetails;
        private readonly Application_Db_Context _context;
        private readonly IFellowshipRepository _repository;
        private Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;

        public ReturnFilesRepository(IExtraDetails extraDetails, Application_Db_Context context, IFellowshipRepository repository, UserManager<IdentityUser> identityUserManager)
        {
            _extraDetails = extraDetails;
            _context = context;
            _repository = repository;
            _IdentityUserManager = identityUserManager;
        }

        public static  string FileName(string Filename)
        {
            string[] parts = Filename.Split('_');
            string filename = parts.Length >= 2 ? parts[1] : Filename;

            return filename;
        }

        public async Task<List<File_Details_Excel_Dto>> GetReturnedFilesData()
        {
            var UserData = 
                                                         (from t1 in _context.appUser
                                                          join t2 in _context.UserDetails on t1.Id equals t2.UserId
                                                          join t3 in _context.File_Details_Excel on t2.UserId equals t3.UserId
                                                          where t3.Status == "File Returned" || t3.Status == "File Returned and GST Bill Submitted"
                                                          select new File_Details_Excel_Dto
                                                          {
                                                              UserId = t1.Id,
                                                              FileId = t3.FileId,
                                                              UserName = t1.UserName,
                                                              GSTType = t3.GSTTye,
                                                              GstNo = t2.GSTNo,
                                                              Email = t1.Email,
                                                              BusinessType = t2.BusinessType,
                                                              UplodedById = t3.UplodedById,
                                                              UplodedByName = _IdentityUserManager.Users.Where(x => x.Id == t3.UplodedById).Select(x => x.UserName).FirstOrDefault(),
                                                              FileName = FileName(t3.FileId),
                                                              Status = t3.Status
                                                          }).ToList();

            return UserData;

        }

        public async Task<List<File_Details_Excel_Dto>> GetReturnedFilesDataForFellowship(string UserID)
        {
            var UserData = 
                                                          (from t1 in _context.appUser
                                                           join t2 in _context.UserDetails on t1.Id equals t2.UserId
                                                           join t3 in _context.File_Details_Excel on t2.UserId equals t3.UserId
                                                           where (t3.Status == "File Returned"||t3.Status== "File Returned and GST Bill Submitted") && t3.UplodedById == UserID
                                                           select new File_Details_Excel_Dto
                                                           {
                                                               UserId = t1.Id,
                                                               FileId = t3.FileId,
                                                               UserName = t1.UserName,
                                                               GSTType = t3.GSTTye,
                                                               GstNo = t2.GSTNo,
                                                               Email = t1.Email,
                                                               BusinessType = t2.BusinessType,
                                                               UplodedById = t3.UplodedById,
                                                               UplodedByName = _IdentityUserManager.Users.Where(x => x.Id == t3.UplodedById).Select(x => x.UserName).FirstOrDefault(),
                                                               FileName = FileName(t3.FileId),
                                                               Status = t3.Status
                                                           }).ToList();

            return UserData;
        }



        public async Task<List<File_Details_Excel_Dto>> ViewReturnFilesDataTable(DataTable_Dto dataTable, string LoginSessionID)
        {
            List<File_Details_Excel_Dto> ReturnFileData = new List<File_Details_Excel_Dto>();
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
                    x.GstNo.ToLower().Contains(searchValue) ||
                    x.Email.ToLower().Contains(searchValue) ||
                    x.BusinessType.ToLower().Contains(searchValue) ||
                    x.FileName.ToString().Contains(searchValue) ||
                    x.UplodedByName.ToString().Contains(searchValue) ||
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
                    case "GstNo":
                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.GstNo).ToList() : ReturnFileData.OrderByDescending(u => u.GstNo).ToList();
                        break;
                    case "Email":
                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.Email).ToList() : ReturnFileData.OrderByDescending(u => u.Email).ToList();
                        break;
                    case "BusinessType":
                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.BusinessType).ToList() : ReturnFileData.OrderByDescending(u => u.BusinessType).ToList();
                        break;
                    case "FileName":
                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.FileName).ToList() : ReturnFileData.OrderByDescending(u => u.FileName).ToList();
                        break;
                    case "UplodedByName":
                        ReturnFileData = dataTable.sortColumnDirection == "asc" ? ReturnFileData.OrderBy(u => u.UplodedByName).ToList() : ReturnFileData.OrderByDescending(u => u.UplodedByName).ToList();
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
