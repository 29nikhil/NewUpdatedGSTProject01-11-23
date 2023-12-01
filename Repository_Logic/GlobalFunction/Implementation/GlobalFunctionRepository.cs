using Data_Access_Layer.Db_Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository_Logic.Dto;
using Repository_Logic.ExportExcelSheet.Interface;
using Repository_Logic.FellowshipDetails.Interface;
using Repository_Logic.FellowshipRepository.Interface;
using Repository_Logic.GlobalFunction.Interface;
using Repository_Logic.TaskAllocation.Interface;
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
        private Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;

        public GlobalFunctionRepository(IFellowshipRepository fellowship, IExtraDetails extraDetails, IExportExcelSheet
            exportExcelSheet, Application_Db_Context context, ITaskAllocation taskAllocation, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> IdentityUserManager)
        {
            _fellowship = fellowship;
            _extraDetails = extraDetails;
            _exportExcelSheet = exportExcelSheet;
            _context = context;
            _taskAllocation = taskAllocation;
            _IdentityUserManager= IdentityUserManager;
        }

        public int AllowcatedTaskDone()
        {
            var PendingCount = _context.AllocatedTasks
                          .Where(x => x.status.Contains( "Done")||x.status.Contains("File Returned")).Count();
            return PendingCount;
        }

        public int AllowcatedTaskTotal()
        {
            var TotalTask= _context.AllocatedTasks.Count();
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
            throw new NotImplementedException();

        }

        public int FellowshipAllowcatedTaskTotal(string id)
        {
            throw new NotImplementedException();
        }

        public int FellowshipReturnFileCount(string id)
        {
            throw new NotImplementedException();
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
                    JoinUserTable_Dto joinUserTable_Dto = new JoinUserTable_Dto();
                    joinUserTable_Dto.FirstName = "Hi CA DADA";

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
            int OrganizationUserCount = _context.UserDetails.Where(x => x.BusinessType.Contains("Individual")).ToList().Count();

            return OrganizationUserCount;
        }

      

        public int OrganizationUserType()
        {
            int OrganizationUserCount = _context.UserDetails.Where(x=>x.BusinessType.Contains("Organization")).ToList().Count();
            
            return OrganizationUserCount;
        }

        public int ReturnFileCountAll()
        {
            var ReturnFileCount = _context.ExcelSheet
                                      .Where(x => x.status == "File Returned")
                                      .GroupBy(x => x.UniqueFileId) // Replace 'SomeProperty' with the actual property you want to group by
                                      .Count();
            return ReturnFileCount;
        }

        public async Task<int> TotalFellowship()
        {
            var usersInRole = await _IdentityUserManager.GetUsersInRoleAsync("Fellowship");
            var con = usersInRole.ToList().Count();
            //  int userCountValue = usersInRole.Count();
            return con;
        }

        public async Task<int> TotalUser()
        {
            var usersInRole = await _IdentityUserManager.GetUsersInRoleAsync("User");
            var con = usersInRole.ToList().Count();
            //  int userCountValue = usersInRole.Count();
            return con;
        }

        public int UserSideDoneChangesFile(string id)
        {
            

            var PendingCount = _context.AllocatedTasks.Where(x => x.status.Contains("Done ") && x.userID == id).Count();
            return PendingCount;


            
        }

        public int UserSidePendingChagesFile(string id)
        {
            var ReturnFileCount = _context.AllocatedTasks
                                                 .Where(x => x.status == "Changes Pending" && x.userID == id)
                                                 .GroupBy(x => x.FileID) // Replace 'SomeProperty' with the actual property you want to group by
                                                 .Count();
            return ReturnFileCount;
        }

        public int UserSideReturnFileCount(string id)
        {
            var ReturnFileCount = _context.ExcelSheet
                                      .Where(x => x.status == "File Returned"&&x.UserID==id)
                                      .GroupBy(x => x.UniqueFileId) // Replace 'SomeProperty' with the actual property you want to group by
                                      .Count();
            return ReturnFileCount;
        }

        public int UserSideUploadFiles(string id)
        {
            var ExcelFileCount = _context.ExcelSheet
                                      .Where(x => x.UserID == id)
                                      .GroupBy(x => x.UniqueFileId) // Replace 'SomeProperty' with the actual property you want to group by
                                      .Count();
            return ExcelFileCount;
        }
    }
}
