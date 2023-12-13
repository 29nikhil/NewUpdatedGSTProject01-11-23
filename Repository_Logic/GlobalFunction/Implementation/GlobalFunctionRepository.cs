﻿using Data_Access_Layer.Db_Context;
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
            var ReturnFileCount = _context.File_Details_Excel
                                      .Where(x => (x.Status == "File Returned" || x.Status == "File Returned and GST Bill Submitted"))
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
            var ReturnFileCount = _context.File_Details_Excel
                                      .Where(x => (x.Status == "File Returned" || x.Status.Contains( "File Returned and GST Bill Submitted")) && x.UserId==id).Count();
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
    }
}
