using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.ModelView
{
    public class File_Details_Excel_ModelView
    {

        public UserOtherDetails otherDetails { get; set; }
        //   public Application_User department { get; set; }
        public Application_User identityUser { get; set; }
        public File_Details_Excel FileDetails { get; set; }
        public UserRegisterLogs UserRegisterLogs { get; set; }
    }
}
