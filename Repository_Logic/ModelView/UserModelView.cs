using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.ModelView
{
    public class UserModelView
    {
        public UserOtherDetails otherDetails { get; set; }
     //   public Application_User department { get; set; }
        public Application_User identityUser { get; set; }

      
    }
}
