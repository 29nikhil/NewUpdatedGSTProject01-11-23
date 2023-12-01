using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
    public class TaskView_Dto
    {
        public string ID { get; set; }

        public string? AllocatedById { get; set; }
        public string? AllocatedByName { get; set; } //Fellowship or Ca Word

        public string? CA_ID { get; set; }
        public string CAName { get; set; }

        public string FileName { get; set; }
        public string UserName { get; set; }

        public string? userID { get; set; }
        public string? FileID { get; set; }

        public string? Remark { get; set; }

        
        public string? status { get; set; }

        public string? Created_date { get; set; }
       
    }
}
