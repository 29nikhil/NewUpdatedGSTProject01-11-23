using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Repository_Logic.Dto;
using Repository_Logic.LoginLogsDataRepository.Interface;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repository_Logic.LoginLogsDataRepository.Implementation
{
    public class LoginLogsImplementation : ILoginLogs
    {
        private readonly Application_Db_Context _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;
        public LoginLogsImplementation(Application_Db_Context context, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> IdentityUserManager)
        {
            _IdentityUserManager = IdentityUserManager;
            _context = context;

        }
        public void insert(LoginLogs_Dto loginDetails)
        {
            LoginLogs loginLogs = new LoginLogs();
            loginLogs.UserID = loginDetails.UserID;
            loginLogs.Message = loginDetails.Message;
            loginLogs.CurrentStatus = loginDetails.CurrentStatus;
            _context.LoginLogs.Add(loginLogs);
            _context.SaveChanges();
        }

        public List<LoginLogs_Dto> GetLoginLogs()
        {

            List<LoginLogs_Dto> loginLogs = _context.LoginLogs.Select(loginlogs => new LoginLogs_Dto
            {

                Id = loginlogs.Id,
                UserID = _IdentityUserManager.Users.Where(x => x.Id == loginlogs.UserID).Select(x => x.UserName).FirstOrDefault(),
                Message = loginlogs.Message,
                CurrentStatus = loginlogs.CurrentStatus,
                Date = loginlogs.FormattedDate,
            }).ToList();
            return loginLogs;
        }

        public List<LoginLogs_Dto> ViewLoginLogsDataTable(DataTable_Dto dataTable_)
        {

            var Records = GetLoginLogs();


            if (!string.IsNullOrEmpty(dataTable_.SearchValue))
            {
                Records = Records.Where(x => x.UserID.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.Message.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.CurrentStatus.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.Date.ToLower().Contains(dataTable_.SearchValue.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(dataTable_.sortColumn) && !string.IsNullOrEmpty(dataTable_.sortColumnDirection))
            {

                IEnumerable<LoginLogs_Dto> Loginlog = Records;
                switch (dataTable_.sortColumn)
                {
                    case "UserID":
                        Loginlog = dataTable_.sortColumnDirection == "asc" ?
                        Loginlog.OrderBy(u => u.UserID) : Loginlog.OrderByDescending(u => u.UserID);
                        break;
                    case "Message":
                        Loginlog = dataTable_.sortColumnDirection == "asc" ?
                        Loginlog.OrderBy(u => u.Message) : Loginlog.OrderByDescending(u => u.Message);
                        break;
                    case "CurrentStatus":
                        Loginlog = dataTable_.sortColumnDirection == "asc" ?
                        Loginlog.OrderBy(u => u.CurrentStatus) : Loginlog.OrderByDescending(u => u.CurrentStatus);
                        break;
                    case "Date":
                        Loginlog = dataTable_.sortColumnDirection == "asc" ?
                              Loginlog.OrderBy(u => u.Date) : Loginlog.OrderByDescending(u => u.Date);
                        break;
                }


                Records = Loginlog.ToList();

            }

            return Records;
        }
    }
}
