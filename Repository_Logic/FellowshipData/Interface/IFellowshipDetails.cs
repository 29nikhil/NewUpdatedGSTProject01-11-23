using Data_Access_Layer.Models;
using Repository_Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.FellowshipDetails.Interface
{
    public interface IFellowshipDetails
    {

        public IEnumerable<Application_User> GetAllFellowship();
       public Application_User GetUser(string id);
        public void DeleteUser(string id);
        public void UpdateUser(Application_User user);
    }
}
