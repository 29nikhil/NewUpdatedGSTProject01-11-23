using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository_Logic.Dto;
using Repository_Logic.ExcelSheetUploadRepository.Interface;
using Repository_Logic.ExportExcelSheet.Interface;
using Repository_Logic.FellowshipDetails.Interface;
using Repository_Logic.FellowshipRepository.Implemantation;
using Repository_Logic.FellowshipRepository.Interface;
using Repository_Logic.GlobalFunction.Interface;
using Repository_Logic.ModelView;
using Repository_Logic.TaskAllocation.Interface;
using Repository_Logic.UserOtherDatails.implementation;
using Repository_Logic.UserOtherDatails.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Providers.Entities;

namespace Repository_Logic.GlobalFunction.Implementation
{
    public class GlobalFunctionRepository : IGlobalFunctionRepository
    {
        private readonly IFellowshipRepository _fellowship;
        private readonly IExtraDetails _extraDetails;
        private readonly IExportExcelSheet _exportExcelSheet;
        private readonly Application_Db_Context _context;
        private readonly ITaskAllocation _taskAllocation;
        private readonly IExcelSheetUpload _ExportData;
        private Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;
        private readonly IFellowshipRepository _fellowshipRepository;
        public GlobalFunctionRepository(IFellowshipRepository fellowship, IExtraDetails extraDetails, IExportExcelSheet
            exportExcelSheet, Application_Db_Context context, ITaskAllocation taskAllocation, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> IdentityUserManager, IExcelSheetUpload ExportData, IFellowshipRepository fellowshipRepository)
        {
            _ExportData = ExportData;
            _fellowship = fellowship;
            _extraDetails = extraDetails;
            _exportExcelSheet = exportExcelSheet;
            _context = context;
            _taskAllocation = taskAllocation;
            _IdentityUserManager = IdentityUserManager;
            _fellowshipRepository = fellowshipRepository;
        }
        public int YearlyGst()
        {
            var totalSum = _context.GSTBills
                .Select(x => (int)x.total)
                .Sum();
            int sum = 0; // Initialize sum outside the loop

           
            return sum;

        }



        public int MonthlyGst()
        {
            var currentMonth = DateTime.Now;
            var sum = _context.GSTBills
                .Where(x => x.CreatedDate == currentMonth)
                .Sum(x => (int)x.total);
            return sum;
        }

        public int AllowcatedTaskDone()
        {
            var PendingCount = _context.TaskAllocated
                          .Where(x => x.status.Contains( "Done")||x.status.Contains("File Returned")).Count();
            return PendingCount;
        }

        public int AllowcatedTaskTotal()
        {
            var TotalTask= _context.TaskAllocated.Count();
            return TotalTask;
        }

        public async Task<CountingUserDataGlobal_Dto> countConfirmemailUser()
        {


            CountingUserDataGlobal_Dto dataGlobal_Dto = new CountingUserDataGlobal_Dto();
            dataGlobal_Dto.ConfirmAccounts = await _extraDetails.GetUserEmailConfirmCountAll();
            dataGlobal_Dto.NotConfirmAccountsUser = await _extraDetails.GetUserNotConfirmedcountAll();
            //   dataGlobal_Dto.NotConfirmAccountsUser = _extraDetails.GetAllUser().Where(x => x.identityUser.EmailConfirmed == false).Count();
            //3 dataGlobal_Dto.TaskCount=_taskAllocation.GetTaskAllocationCount();
            return dataGlobal_Dto;

        }

        public int countUser()
        {
            throw new NotImplementedException();
        }

        public int FellowshipAllowcatedTask(string id)
        {
            var PendingCount = _context.TaskAllocated.Where(x => x.status.Contains("Changes Done ") && x.AllocatedById== id).Count();
            return PendingCount;


            
        }

        public int FellowshipTotalFileUploded(string id)
        {
            var ReturnFileCount = _context.TaskAllocated.Where(x => x.AllocatedById== id).Count();
            // Replace 'SomeProperty' with the actual property you want to group by.Count();
            return ReturnFileCount;

        }

        public int FellowshipReturnFileCount(string id)
        {
            var ReturnFileCount = _context.File_Details_Excel
                                      .Where(x => (x.Status == "File Returned"||x.Status == "File Returned and GST Bill Submitted") &&x.UplodedById==id)
                                      .Count();
            return ReturnFileCount;
        }

        public async Task<JoinUserTable_Dto> GetUserDataNavBar(string Id)
        {
            var user = await _IdentityUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                bool isInRole = await _IdentityUserManager.IsInRoleAsync(user, "User");
                bool isInRoleCa = await _IdentityUserManager.IsInRoleAsync(user, "CA");

                if (isInRole)
                {
                    var Userdata = _extraDetails.GetUser(Id);
                    return Userdata;
                }
                else if (isInRoleCa)
                {
                    var Userdata = _context.appUser.Where(x=>x.Id==Id).FirstOrDefault();
                    JoinUserTable_Dto joinUserTable_Dto = new JoinUserTable_Dto();
                    joinUserTable_Dto.FirstName = "Hi CA "+Userdata.FirstName;

                    return joinUserTable_Dto;
                }
                else 
                {
                    JoinUserTable_Dto joinUserTable_Dto = new JoinUserTable_Dto();
                    var Userdata = _fellowship.GetFellowShipṚeccord(Id);
                    joinUserTable_Dto.FirstName = "Hi " + Userdata.FirstName;

                    return joinUserTable_Dto;
                }

            }

            // If the user is not in the "User" role or if the user is not found, return null.
            return null;


        }

        public int IndividualUserType()
        {
            //int OrganizationUserCount = _context.UserDetails.Where(x => x.BusinessType.Contains("Individual")).ToList().Count();

            //return OrganizationUserCount;
            Application_User IdentityUserData1 = new Application_User();
            var IdentityUserData = _context.appUser.ToList();
            var UserOtherData = _context.UserDetails.ToList();



            var userdetails = (from iUser in _context.appUser
                                    // join rUser in _context.UserRoles
                                    // on iUser.Id equals rUser.UserId
                                where iUser.IsDeleted == false 
                                join ouser in _context.UserDetails

                                   on iUser.Id equals ouser.UserId
                                 into table2
                              
                               from oueser in table2.ToList()
                               where oueser.BusinessType == "Individual"
                               select new UserModelView
                                {
                                    otherDetails = oueser,
                                    identityUser = iUser,


                                }).ToList();


            return userdetails.Count();

        }

      

        public int OrganizationUserType()
        {
            Application_User IdentityUserData1 = new Application_User();
            var IdentityUserData = _context.appUser.ToList();
            var UserOtherData = _context.UserDetails.ToList();



            var userdetails = (from iUser in _context.appUser
                                   // join rUser in _context.UserRoles
                                   // on iUser.Id equals rUser.UserId
                               where iUser.IsDeleted == false
                               join ouser in _context.UserDetails

                                  on iUser.Id equals ouser.UserId
                                into table2

                               from oueser in table2.ToList()
                               where oueser.BusinessType == "Organization"
                               select new UserModelView
                               {
                                   otherDetails = oueser,
                                   identityUser = iUser,


                               }).ToList();


            return userdetails.Count();
        }

        public int ReturnFileCountAll()
        {
            var ReturnFileCount = _context.File_Details_Excel
                                      .Where(x => (x.Status == "File Returned" || x.Status == "File Returned and GST Bill Submitted"))
                                      .Count();
            return ReturnFileCount;
        }

        public async Task<int> TotalFellowship()
        {
             var UserDetails=   _fellowshipRepository.GetAllFellowshipRecord();


            var filteredUsers = UserDetails.Where(user => user.IsDeleted == false).ToList();

            return filteredUsers.Count;
        }

        public async Task<int> TotalUser()
        {
            var usersInRole = _extraDetails.GetAllUser();
           
            var con = usersInRole.ToList().Count();
            //  int userCountValue = usersInRole.Count();
            return con;
        }

        public int UserSideDoneChangesFile(string id)
        {
            

            var PendingCount = _context.TaskAllocated.Where(x => x.status.Contains("Changes Done") && x.userID == id).Count();
            return PendingCount;


            
        }

        public int UserSidePendingChagesFile(string id)
        {
            var ReturnFileCount = _context.TaskAllocated.Where(x => x.status.Contains("Changes Pending") && x.userID == id).Count();
                                                 // Replace 'SomeProperty' with the actual property you want to group by.Count();
            return ReturnFileCount;
        }

        public int UserSideReturnFileCount(string id)
        {
            var ReturnFileCount = _context.File_Details_Excel.Where(x => (x.Status == "File Returned") && x.UserId==id).Count();
            // Replace 'SomeProperty' with the actual property you want to group by
            return ReturnFileCount;
        }

        public int UserSideUploadFiles(string id)
        {
            var ExcelFileCount = _context.File_Details_Excel
                                      .Where(x => x.UserId == id).Count();
            // Replace 'SomeProperty' with the actual property you want to group by

            return ExcelFileCount;
        }

        public async Task<int> countTotalFiles(string LoginSessionID)
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
                  where  T3.UplodedById == LoginSessionID

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
            return UserData.Count;
        }
        public static string FileName(string Filename)
        {
            string[] parts = Filename.Split('_');
            string filename = parts.Length >= 2 ? parts[1] : Filename;

            return filename;
        }
        public async Task<int> ReturnFilesForFellowship(string LoginSessionID)
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
                  where T3.UplodedById == LoginSessionID && T3.Status == "File Returned"
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
                    where T3.Status == "File Returned"
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



            return UserData.Count;
        }

        public async Task<int> countPendingTaskForFellowship(string LoginSessionID)
        {
          
           
            var Records =await _taskAllocation.TaskListAsync(LoginSessionID);

            var extractedRecords = Records.Where(record => record.status == "Changes Pending").ToList();

            return extractedRecords.Count;
        }

        public int countTotalFilesForUser(string LoginSessionID)
        {
          var  UserData = (
                  from T1 in _context.appUser
                  join T2 in _context.UserDetails on T1.Id equals T2.UserId
                  join T3 in _context.File_Details_Excel on T2.UserId equals T3.UserId
                  join T4 in _context.userResistorLogs on T3.UserId equals T4.UserID
                  where T3.UserId == LoginSessionID 
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

            return UserData.Count;
        }

        public int countReturnedFilesAndGSTBillsSubmittedForUser(string LoginSessionID)
        {
            var UserData = (
                  from T1 in _context.appUser
                  join T2 in _context.UserDetails on T1.Id equals T2.UserId
                  join T3 in _context.File_Details_Excel on T2.UserId equals T3.UserId
                  join T4 in _context.userResistorLogs on T3.UserId equals T4.UserID
                  where T3.UserId == LoginSessionID && T3.Status == "File Returned and GST Bill Submitted"
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

            return UserData.Count;
        }

        public int CountReturnedFilesForUser(string LoginSessionID)
        {
            var UserData = (
                  from T1 in _context.appUser
                  join T2 in _context.UserDetails on T1.Id equals T2.UserId
                  join T3 in _context.File_Details_Excel on T2.UserId equals T3.UserId
                  join T4 in _context.userResistorLogs on T3.UserId equals T4.UserID
                  where T3.UserId == LoginSessionID && T3.Status == "File Returned"
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

            return UserData.Count;
        }

        public async Task<int> countReturnedFilesAndGSTBillsSubmittedForFellowship(string LoginSessionID)
        {
         var  UserData = (
                  from T1 in _context.appUser
                  join T2 in _context.UserDetails on T1.Id equals T2.UserId
                  join T3 in _context.File_Details_Excel on T2.UserId equals T3.UserId
                  join T4 in _context.userResistorLogs on T3.UserId equals T4.UserID
                  where T3.UplodedById == LoginSessionID && T3.Status == "File Returned and GST Bill Submitted"
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

          return  UserData.Count;
        }

        public async Task<int> countFilesReturnedAndGSTBillsSubmittedForCA()
        {
            var UserData = (
                 from T1 in _context.appUser
                 join T2 in _context.UserDetails on T1.Id equals T2.UserId
                 join T3 in _context.File_Details_Excel on T2.UserId equals T3.UserId
                 join T4 in _context.userResistorLogs on T3.UserId equals T4.UserID
                 where T3.Status == "File Returned and GST Bill Submitted"
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

            return UserData.Count;
        }

        public async Task<int> countFilesReturnedAndGSTBillsNotSubmittedForCA()
        {

            var UserData = (
                from T1 in _context.appUser
                join T2 in _context.UserDetails on T1.Id equals T2.UserId
                join T3 in _context.File_Details_Excel on T2.UserId equals T3.UserId
                join T4 in _context.userResistorLogs on T3.UserId equals T4.UserID
                where T3.Status == "File Returned"
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

            return UserData.Count;
        }

      
    }
}
