using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
    public class TaskList_Dto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string userId { get; set; }
        public string FileName { get; set; }
        public string uniqueFileId { get; set; }
        public string Remark { get; set; }

        public string CAName { get; set; }

        public string status { get; set; }

        public string SessionID { get; set; }
        public DateTime Created_Date { get; set; }
        public string date { get; set; }
    }
}
