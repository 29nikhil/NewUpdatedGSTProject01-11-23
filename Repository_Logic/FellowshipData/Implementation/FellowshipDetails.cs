using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Repository_Logic.FellowshipDetails.Interface;
using Repository_Logic.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.FellowshipDetails.Implementation
{
    public class FellowshipDetails : IFellowshipDetails
    {

        private readonly Application_Db_Context _context;
        //string constring = "Server=NAROLA-50\\SQLEXPRESS2022;Database=The_GST_22;Trusted_Connection=true ;Encrypt=false;TrustServerCertificate=true";

        public FellowshipDetails(Application_Db_Context context, string constring)
        {
            _context = context;
            //this.constring = constring;
        }

        public void DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Application_User> GetAllFellowship()
        {
            var FellowshipList = _context.appUser.ToList();
            var Role = _context.Roles.Where(x => x.Name == "FELLOWSHIP");
            var rolid = _context.Roles.ToList();
            string Roleid = Role.FirstOrDefault()?.Id;
            var UserRoles = _context.UserRoles.ToList();


            var procart = (from UserRole in UserRoles

                           join User1 in FellowshipList on UserRole.UserId equals User1.Id into table1
                           from c in table1.ToList()
                           where UserRole.RoleId == Roleid

                           select new Application_User
                           {
                               Id = c.Id,
                               FirstName = c.FirstName,
                               MiddleName = c.MiddleName,
                               LastName = c.LastName,
                               Address = c.Address,
                               Email = c.Email,
                               PhoneNumber = c.PhoneNumber,
                               city = c.city,
                               Country = c.Country

                           }).ToList();



            return procart;
        }

        public Application_User GetUser(string id)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(Application_User user)
        {
            throw new NotImplementedException();
        }
    }
}
