using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using Repository_Logic.Dto;
using Repository_Logic.FileUploads.Implementation;
using Repository_Logic.FileUploads.Interface;
using Repository_Logic.ModelView;
using Repository_Logic.UserOtherDatails.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Providers.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repository_Logic.UserOtherDatails.implementation
{
    public class ExtraDetails : IExtraDetails
    {
        private readonly Application_Db_Context _context;
        private DbSet<UserModelView> UserEntity;
        private Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;
        //  string constring = "Server=NAROLA-50\\SQLEXPRESS2022;Database=The_GST_23;Trusted_Connection=true;Encrypt=false;TrustServerCertificate=true";
        string constring = "Server=NIKHIL\\SQLEXPRESS;Database=The_GST_30;Trusted_Connection=true;Encrypt=false;TrustServerCertificate=true";

        private readonly IFileRepository fileRepository;
        public ExtraDetails(Application_Db_Context context, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> IdentityUserManager, IFileRepository fileRepository)
        {
            _context = context;
            UserEntity = _context.Set<UserModelView>();
            _IdentityUserManager = IdentityUserManager;
            this.fileRepository = fileRepository;

        }
        UserOtherDetails userOtherDetails = new UserOtherDetails();
        public async Task Add(UserOtherDetails_Dto otherDetails)
        {

            FilePathReturn_Dto filePathReturn_Dto = new FilePathReturn_Dto();
            filePathReturn_Dto = fileRepository.UploadData(otherDetails.UploadAdharPath, otherDetails.UploadPanPath);



            // userOtherDetails.UserId = otherDetails.UserId;
            userOtherDetails.UploadPAN = filePathReturn_Dto.UploadPanPath;
            userOtherDetails.UploadAadhar = filePathReturn_Dto.UploadAdharPath;
            userOtherDetails.BusinessType = otherDetails.BusinessType;
            userOtherDetails.GSTNo = otherDetails.GSTNo;
            userOtherDetails.PANNo = otherDetails.PANNo;
            userOtherDetails.AdharNo = otherDetails.AdharNo;
            userOtherDetails.website = otherDetails.website;
            userOtherDetails.UserId = otherDetails.UserId;
            await _context.AddAsync(userOtherDetails);
            _context.SaveChanges();
        }

        public IEnumerable<UserModelView> GetAllUser()
        {

            Application_User IdentityUserData1 = new Application_User();
            var IdentityUserData = _context.appUser.ToList();
            var UserOtherData = _context.UserDetails.ToList();



            var userdetails1 = (from iUser in _context.appUser
                                    // join rUser in _context.UserRoles
                                    // on iUser.Id equals rUser.UserId
                                where iUser.IsDeleted == false
                                join ouser in _context.UserDetails
                                   on iUser.Id equals ouser.UserId

                                   into table2
                                from oueser in table2.ToList()

                                select new UserModelView
                                {
                                    otherDetails = oueser,
                                    identityUser = iUser,


                                }).ToList();
            return userdetails1;
        }


        public async void DeleteUser(string id)
        {

            //SqlConnection con = new SqlConnection(constring);
            //string pname = "Delete_User";
            //con.Open();
            //SqlCommand cmd = new SqlCommand(pname, con);
            //cmd.CommandType = System.Data.CommandType.StoredProcedure;


            //cmd.Parameters.AddWithValue("@Userid", id);


            //cmd.ExecuteNonQuery();
            //con.Close();

            var UserRecord = _context.appUser.Where(model => model.Id == id).FirstOrDefault();
            UserRecord.IsDeleted = true;

            _context.Entry(UserRecord).State = EntityState.Modified;
            _context.SaveChanges();

        }



        public async void UpdateUser(JoinUserTable_Dto user)
        {



            SqlConnection con = new SqlConnection(constring);
            string pname = "Edit_User";
            con.Open();
            SqlCommand cmd = new SqlCommand(pname, con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@Userid", user.Id);
            cmd.Parameters.AddWithValue("@gstNo", user.GSTNo);
            cmd.Parameters.AddWithValue("@PanNo", user.PANNo);
            cmd.Parameters.AddWithValue("@AdharNo", user.AdharNo);
            if (user.AdharFile != null && user.PanFile != null)
            {
                var statusfile = fileRepository.DeleteAdhar(user.UploadAadhar);

                var AdharstatusFile = fileRepository.UpdateAdhar(user.AdharFile);
                var statusfile1 = fileRepository.DeletePan(user.UploadPAN);

                var PanstatusFile = fileRepository.UpdatePan(user.PanFile);
                cmd.Parameters.AddWithValue("@UploadPan", PanstatusFile);

                cmd.Parameters.AddWithValue("@UploadAdhar", AdharstatusFile);

            }
            else if (user.AdharFile != null)
            {
                var statusfile = fileRepository.DeleteAdhar(user.UploadAadhar);

                var AdharstatusFile = fileRepository.UpdateAdhar(user.AdharFile);

                cmd.Parameters.AddWithValue("@UploadAdhar", AdharstatusFile);
                cmd.Parameters.AddWithValue("@UploadPan", user.UploadPAN);

            }
            else if (user.PanFile != null)
            {
                var statusfile = fileRepository.DeletePan(user.UploadPAN);

                var PanstatusFile = fileRepository.UpdatePan(user.PanFile);
                cmd.Parameters.AddWithValue("@UploadPan", PanstatusFile);
                cmd.Parameters.AddWithValue("@UploadAdhar", user.UploadAadhar);

            }
            else
            {
                cmd.Parameters.AddWithValue("@UploadPan", user.UploadPAN);
                cmd.Parameters.AddWithValue("@UploadAdhar", user.UploadAadhar);
            }

            cmd.Parameters.AddWithValue("@Bussiness", user.BusinessType);
            cmd.Parameters.AddWithValue("@WebSite", user.website);
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

        public JoinUserTable_Dto GetUser(string id)
        {

            var data = _context.UserDetails.Where(x => x.UserId == id).FirstOrDefault();
            var data1 = _context.appUser.Where(x => x.Id == id).FirstOrDefault();
            // Create an instance of your view model
            var viewModel = new JoinUserTable_Dto
            {

                Id = data1.Id,
                FirstName = data1.FirstName,
                LastName = data1.LastName,
                MiddleName = data1.MiddleName,
                PhoneNumber = data1.PhoneNumber,
                Email = data1.Email,
                city = data1.city,
                Address = data1.Address,
                BusinessType = data.BusinessType,
                GSTNo = data.GSTNo,
                PANNo = data.PANNo,
                AdharNo = data.AdharNo,
                UploadAadhar = data.UploadAadhar,
                UploadPAN = data.UploadPAN,
                UserStatus = data1.UserStatus,
                website = data.website,
                Date = data1.Date,
                UserId = data.UserId,
                Country = data1.Country,
                userName = data1.UserName




                // Map other properties as needed
            };

            return viewModel;


        }




        public IEnumerable<JoinUserTable_Dto> GetAllUserList()
        {
            var IdentityUserData = _context.appUser.ToList();
            var UserOtherData = _context.UserDetails.ToList();



            var userdetails1 = (from data1 in _context.appUser
                                    // join rUser in _context.UserRoles
                                    // on iUser.Id equals rUser.UserId
                                where data1.IsDeleted == false
                                join data in _context.UserDetails
                                   on data1.Id equals data.UserId

                                   into table2
                                from data in table2.ToList()

                                select new JoinUserTable_Dto
                                {
                                    Id = data1.Id,
                                    FirstName = data1.FirstName,
                                    LastName = data1.LastName,
                                    MiddleName = data1.MiddleName,
                                    Email = data1.Email,
                                    city = data1.city,
                                    Address = data1.Address,
                                    BusinessType = data.BusinessType,
                                    GSTNo = data.GSTNo,
                                    PANNo = data.PANNo,
                                    AdharNo = data.AdharNo,
                                    UploadAadhar = data.UploadAadhar,
                                    UploadPAN = data.UploadPAN,
                                    UserStatus = data1.UserStatus,
                                    website = data.website,
                                    Date = data1.Date,
                                    UserId = data.UserId,
                                    Country = data1.Country,
                                    PhoneNumber = data1.PhoneNumber,
                                    Confirm = data1.EmailConfirmed

                                }).ToList();
            return userdetails1;
        }

        public async Task<int> GetUserConfirmedCountAll()
        {
            var usersInRole = await _IdentityUserManager.GetUsersInRoleAsync("User");
            var con = usersInRole.Where(x => x.EmailConfirmed == true).Count();
            //  int userCountValue = usersInRole.Count();
            return con;
        }

        public async Task<int> GetUserNotConfirmedcountAll()
        {
            var usersInRole = await _IdentityUserManager.GetUsersInRoleAsync("User");
            var con = usersInRole.Where(x => x.EmailConfirmed == false).ToList().Count();
            return con;
        }

        public void UpdateEmailConfirmation(string UserId)
        {

            SqlConnection con = new SqlConnection(constring);
            string pname = "EmailConfirm";
            con.Open();
            SqlCommand cmd = new SqlCommand(pname, con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@UserId", UserId);
            cmd.Parameters.AddWithValue("@EmailConfirmation", false);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public async Task<int> GetUserEmailConfirmCountAll()
        {
            var usersInRole = await _IdentityUserManager.GetUsersInRoleAsync("User");
            var con = usersInRole.Where(x => x.EmailConfirmed == true).ToList().Count();
            //  int userCountValue = usersInRole.Count();
            return con;
        }

        public IEnumerable<Application_User> ShowAllUsers()
        {
            var Userlist = _context.appUser.ToList();
            return Userlist;
        }




        public async Task<Application_User> ShowInfirmationUsers(string Userid)
        {
            var Userlist = _context.appUser.Where(x => x.Id == Userid).FirstOrDefault();
            return Userlist;
        }



        public string AvaibleEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return "{\"isValid\": false, \"message\": \"Email is not provided. Please enter an email.\"}";
            }
            else
            {
                bool emailExists = _context.appUser.Any(x => x.Email == email);

                if (emailExists)
                {
                    return "{\"isValid\": false, \"message\": \"This email is already taken. Please enter a new email.\"}";
                }
                else
                {
                    return "{\"isValid\": true, \"message\": \"Email is available.\"}";
                }
            }
        }


        public string AvaibleGstNo(string gstNo)
        {
            if (string.IsNullOrEmpty(gstNo))
            {
                return "{\"isValid\": false, \"message\": \"Email is not provided. Please enter an email.\"}";
            }
            else
            {
                bool emailExists = _context.UserDetails.Any(x => x.GSTNo== gstNo);

                if (emailExists)
                {
                    return "{\"isValid\": false, \"message\": \"This email is already taken. Please enter a new email.\"}";
                }
                else
                {
                    return "{\"isValid\": true, \"message\": \"Email is available.\"}";
                }
            }
        }

    }
}
