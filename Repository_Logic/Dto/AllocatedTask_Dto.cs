using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
    public class AllocatedTask_Dto
    {

        public string userID { get; set; }
        public string SessionID { get; set; }

        public string FileID { get; set; }

        public string Remark { get; set; }

        public string Login_SessionID { get; set; }

        public string status { get; set; }
        public DateTime Created_date { get; set; }
        public string date {get; set; }
    }
    
}
