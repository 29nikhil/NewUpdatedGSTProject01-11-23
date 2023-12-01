using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Repository_Logic.Dto;
using Repository_Logic.ModelView;
using Repository_Logic.RegistorLogsRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static OfficeOpenXml.ExcelErrorValue;

namespace Repository_Logic.RegistorLogsRepository.Implementation
{
    public class RegisterLogs : IRegisterLogs
    {
        private  Application_Db_Context _context;
        private static Application_Db_Context _context1;

        private Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;

        public RegisterLogs(Application_Db_Context context, UserManager<IdentityUser> identityUserManager, Application_Db_Context context1)
        {
            _context = context;
            _IdentityUserManager = identityUserManager;
            _context1 = context1;
        }



        public IEnumerable<RegisterLogVIew_Dto> GetAllRegistorLogs()
        {

            var userdetails1 = (from logTable in _context.userResistorLogs
                                join oueser in _context.appUser on logTable.UserID equals oueser.Id
                                select new RegisterLogVIew_Dto
                                {
                                    UserID = logTable.UserID,
                                    RegistorById = logTable.RegistorById,
                                    CA_ID = logTable.CA_ID,
                                    userFullName = oueser.FirstName + " " + oueser.LastName,
                                    RegistorByName = logTable.RegistorByUserName,
                                    CA_Name = logTable.CA_Email,
                                    RegistorStatus = logTable.UserRole,
                                    Date = logTable.FormattedDate

                                }).ToList();

            return userdetails1;
            //throw new NotImplementedException();

        }
        public static string AccessRoleName(string ID)
        {
            // Check if the ID exists in the UserDetails collection
            var exists = _context1.UserDetails.Any(x => x.Equals(ID));

            // Return a string based on whether the ID exists or not
            if (exists!=null)
            {
                exists = true;
                return "USER";
            }
            else
            {
                return "FELLOWSHIP";
            }
        }






        public static string AccessRoleNameAsync(string ID)
        {
            return "_context";
        }
        
      
        //Save Fellowship Log
        public async Task<string> SaveRegistorLogsFellowship(string userId, string RegistorById)
        {
            UserRegisterLogs resistorLogs = new UserRegisterLogs();

            var user = await _IdentityUserManager.FindByIdAsync(RegistorById);
            var isInRoleCA = await _IdentityUserManager.IsInRoleAsync(user, "CA");
            if (isInRoleCA )
            {
                var userNAme = await _IdentityUserManager.FindByIdAsync(RegistorById);

                resistorLogs.CA_ID = RegistorById;
                resistorLogs.RegistorById = RegistorById;
                resistorLogs.UserID = userId;
                resistorLogs.RegistorByUserName = "CA";
                resistorLogs.CA_Email = userNAme.Email;
                resistorLogs.RegistorByEmail = userNAme.Email;
                resistorLogs.CreatedDate = DateTime.Now;
                resistorLogs.UserRole = "Fellowship";
                _context.userResistorLogs.Add(resistorLogs);
                _context.SaveChanges();
                return "success";

            }
            else
            {
                return "Fail";

            }
        }
        //Save User Log
        public async Task<string> SaveRegistorLogsUser(string userId, string RegistorById)
        {
            var user = await _IdentityUserManager.FindByIdAsync(RegistorById);
            var isInRole = await _IdentityUserManager.IsInRoleAsync(user, "Fellowship");
            var isInRoleCA = await _IdentityUserManager.IsInRoleAsync(user, "CA");
            UserRegisterLogs resistorLogs = new UserRegisterLogs();

            if (isInRoleCA)
            {
                var userNAme = await _IdentityUserManager.FindByIdAsync(RegistorById);

                resistorLogs.CA_ID = RegistorById;
                resistorLogs.RegistorById = RegistorById;
                resistorLogs.UserID=userId;
                resistorLogs.RegistorByEmail = userNAme.Email;
                resistorLogs.RegistorByUserName = "CA";
                resistorLogs.UserRole = "User";
                resistorLogs.CA_Email = userNAme.Email;
                resistorLogs.CreatedDate = DateTime.Now;
                _context.userResistorLogs.Add(resistorLogs);
                _context.SaveChanges();
                return "success";
                
                
            }
            else if(isInRole)
            {
                var userNAme = await _IdentityUserManager.FindByIdAsync(RegistorById);
                var RegistorByName =  _context.appUser.Where(x => x.Id == RegistorById).FirstOrDefault();


                var GetCAId =_context.userResistorLogs.Where(x=>x.UserID==RegistorById).FirstOrDefault();
                resistorLogs.CA_ID =GetCAId.CA_ID;
                resistorLogs.CA_Email = GetCAId.CA_Email;
                resistorLogs.RegistorById = RegistorById;
                resistorLogs.RegistorByEmail = GetCAId.CA_Email;
                resistorLogs.UserID = userId;
                resistorLogs.RegistorByUserName =RegistorByName.FirstName+" "+RegistorByName.LastName;
                resistorLogs.UserRole = "User";
                
                resistorLogs.CreatedDate = DateTime.Now;
                _context.userResistorLogs.Add(resistorLogs);
                _context.SaveChanges();
                return "success";

            }
            else
            {
                return "";
            }
        }


        public async Task<List<RegisterLogVIew_Dto>> ViewRegisterLogsDataTable(DataTable_Dto dataTable_ )
        {
           
            var Records = new List<RegisterLogVIew_Dto>();
            Records = (List<RegisterLogVIew_Dto>)GetAllRegistorLogs();


            if (!string.IsNullOrEmpty(dataTable_.SearchValue))
            {
                Records = Records.Where(x => x.userFullName.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.RegistorByName.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.CA_Name.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.Date.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.RegistorStatus.ToLower().Contains(dataTable_.SearchValue.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(dataTable_.sortColumn) && !string.IsNullOrEmpty(dataTable_.sortColumnDirection))
            {

                IEnumerable<RegisterLogVIew_Dto> Registorlog = Records;

                switch (dataTable_.sortColumn)
                {

                    case "userFullName":
                        Registorlog = dataTable_.sortColumnDirection == "asc" ?
                                Registorlog.OrderBy(u => u.userFullName) : Registorlog.OrderByDescending(u => u.userFullName);
                        break;

                    case "RegistorByName":
                        Registorlog = dataTable_.sortColumnDirection == "asc" ?
                                Registorlog.OrderBy(u => u.RegistorByName) : Registorlog.OrderByDescending(u => u.RegistorByName);
                        break;

                    case "CA_Name":
                        Registorlog = dataTable_.sortColumnDirection == "asc" ?
                                Registorlog.OrderBy(u => u.CA_Name) : Registorlog.OrderByDescending(u => u.CA_Name);
                        break;

                    case "SessionID":
                        Registorlog = dataTable_.sortColumnDirection == "asc" ?
                                Registorlog.OrderBy(u => u.Date) : Registorlog.OrderByDescending(u => u.Date);
                        break;
                    case "Remark":
                        Registorlog = dataTable_.sortColumnDirection == "asc" ?
                                Registorlog.OrderBy(u => u.RegistorStatus) : Registorlog.OrderByDescending(u => u.RegistorStatus);
                        break;
                   
                }



                Records = Registorlog.ToList();

            }

            return Records;
        }
    }
}
