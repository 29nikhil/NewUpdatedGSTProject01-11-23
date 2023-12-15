using Data_Access_Layer.Connection_String;
using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository_Logic.Dto;
using Repository_Logic.FellowshipRepository.Interface;
using Repository_Logic.UserOtherDatails.implementation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Providers.Entities;

namespace Repository_Logic.FellowshipRepository.Implemantation
{
    public class FellowshipRepository : IFellowshipRepository
    {
        private readonly Application_Db_Context _context;
        private readonly Connection_StringData _connectionString;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public FellowshipRepository(Application_Db_Context context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager=userManager;
        }

        public void DeleteFellowship(string id)
        {


         
                var UserRecord = _context.appUser.Where(model => model.Id == id).FirstOrDefault();
                UserRecord.IsDeleted = true;

                _context.Entry(UserRecord).State = EntityState.Modified;
                _context.SaveChanges();
            
        }

        public IEnumerable<Application_User> GetAllFellowshipRecord()
        {
            var fellowshipRole = _context.Roles.SingleOrDefault(x => x.Name == "FELLOWSHIP");

            if (fellowshipRole != null)
            {
                var fellowshipUsers = _context.UserRoles
                    .Where(ur => ur.RoleId == fellowshipRole.Id)
                    .Select(ur => ur.UserId)
                    .ToList();

                var procart = _context.appUser
                    .Where(user => fellowshipUsers.Contains(user.Id)&&user.IsDeleted==false)
                    .Select(user => new Application_User
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        MiddleName = user.MiddleName,
                        LastName = user.LastName,
                        Address = user.Address,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        city = user.city,
                        Country = user.Country
                    })
                    .ToList();
                return procart;

            }
            return null;
        }

        public IEnumerable<Application_User> GetAllFellowshipRecord(DataTable_Dto dataTable_Dto)
        {
            var UsersInRole = GetAllFellowshipRecord();

            
            // Filter the data based on search criteria
            if (!string.IsNullOrEmpty(dataTable_Dto.SearchValue))
            {
                string searchValue = dataTable_Dto.SearchValue.ToLower();
                UsersInRole = UsersInRole.Where(x =>
                    x.FirstName.ToLower().Contains(searchValue) ||
                    x.MiddleName.ToLower().Contains(searchValue) ||
                    x.LastName.ToLower().Contains(searchValue) ||
                    x.Country.ToLower().Contains(searchValue) ||
                    x.city.ToLower().Contains(searchValue) ||
                    x.EmailConfirmed ||
                    x.Email.ToLower().Contains(searchValue) ||
                    x.Address.ToLower().Contains(searchValue) ||
                    x.PhoneNumber.Contains(searchValue)).ToList();
            }

            dataTable_Dto.TotalRecord = UsersInRole.Count();

            // Sort the data based on column and direction
            if (!string.IsNullOrEmpty(dataTable_Dto.sortColumn) && !string.IsNullOrEmpty(dataTable_Dto.sortColumnDirection))
            {
                Func<Application_User, string> sortKeySelector = GetSortKeySelector(dataTable_Dto.sortColumn);

                if (dataTable_Dto.sortColumnDirection == "asc")
                {
                    UsersInRole = UsersInRole.OrderBy(sortKeySelector).ToList();
                }
                else
                {
                    UsersInRole = UsersInRole.OrderByDescending(sortKeySelector).ToList();
                }
            }

            dataTable_Dto.FilterRecord = UsersInRole.Count();

            // Perform pagination
            var FellowshipList = UsersInRole.Skip(dataTable_Dto.Skip).Take(dataTable_Dto.PageSize).ToList();

            return FellowshipList;


        }
        private Func<Application_User, string> GetSortKeySelector(string sortColumn)
        {
            return sortColumn switch
            {
                "FirstName" => u => u.FirstName,
                "MiddleName" => u => u.MiddleName,
                "LastName" => u => u.LastName,
                "Country" => u => u.Country,
                "city" => u => u.city,
                "Email" => u => u.Email,
                "Address" => u => u.Address,
                "PhoneNumber" => u => u.PhoneNumber,
                "Id" => u => u.Id,
                "EmailConfirmed" => u => u.EmailConfirmed.ToString(),
                _ => null
            };
        }

        public Application_User_Dto GetFellowShipṚeccord(string id)
        {
            var data1 = _context.appUser.Where(x => x.Id == id).FirstOrDefault();

            Application_User_Dto application_User_Dto=new Application_User_Dto();
            application_User_Dto.Id = data1.Id;
            application_User_Dto.FirstName = data1.FirstName;
            application_User_Dto.MiddleName = data1.MiddleName;
            application_User_Dto.LastName = data1.LastName;
            application_User_Dto.PhoneNumber= data1.PhoneNumber;
            application_User_Dto.Country = data1.Country;
            application_User_Dto.Email= data1.Email;
            application_User_Dto.Address= data1.Address;
            application_User_Dto.city = data1.city;
            application_User_Dto.Date = data1.Date;
        
            return application_User_Dto;
        }

        public void UpdateFellowship(Application_User_Dto user)
        {
            
         string constring = "Server=NAROLA-44\\SQLEXPRESS2022;Database=The_GST_31;Trusted_Connection=true;Encrypt=false;TrustServerCertificate=true";
        //     string constring = "Server=NIKHIL\\SQLEXPRESS;Database=The_GST_30;Trusted_Connection=true ;Encrypt=false;TrustServerCertificate=true";

            SqlConnection con = new SqlConnection(constring);
            string pname = "Edit_FellowShip1";
            con.Open();
            SqlCommand cmd = new SqlCommand(pname, con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@Userid", user.Id);
            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@MiddleName", user.MiddleName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
            cmd.Parameters.AddWithValue("@Address", user.Address);
            cmd.Parameters.AddWithValue("@Country", user.Country);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@UserStatus", "Not Return");
            cmd.Parameters.AddWithValue("@city", user.city);

            cmd.ExecuteNonQuery();
            con.Close();


        }

        public string EmailChange(Application_User user)
        {
            var findUserEmail = GetFellowShipṚeccord(user.Id);

            if(findUserEmail.Email!=user.Email)
            {
                var a= _userManager.UpdateAsync(user);

            }
            else
            {
                return user.Email;

            }
            return user.Email;
        }


        public async Task<string> GetFellowshisession(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var isInRoleFellowship = await _userManager.IsInRoleAsync(user, "Fellowship");
            var isInRoleCA = await _userManager.IsInRoleAsync(user, "CA");

            string userName = "";
            if ( isInRoleCA)
            {
                userName = user.UserName;
                return userName;
            }
            else
            {
                var a = GetFellowShipṚeccord(id);
                userName=a.FirstName+" "+a.MiddleName+" "+a.LastName;
                return userName;
            }

        }



        public string GetIDByUserName(string UserName)
        {
            var Record = _userManager.Users.Where(x => x.UserName == UserName).FirstOrDefault();
            var ID = Record.Id;
            return ID;
        }

        public string GetCA(string id)
        {
            var Record = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();

            var UserName = Record.UserName;
            return UserName;
        }


    }
}
