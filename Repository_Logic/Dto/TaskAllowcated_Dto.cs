using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
    public class TaskAllowcated_Dto
    {
        public string ID { get; set; }

        public string? AllocatedById { get; set; }
        public string? CA_ID { get; set; }


        public string? userID { get; set; }
        public string? FileID { get; set; }

        public string? Remark { get; set; }


        public string? status { get; set; }

        public DateTime? Created_date { get; set; } = DateTime.Now;
        public DateTime? Modified_date { get; set; }
        public bool? IsDeleted { get; set; }

        public bool? IsTestdata { get; set; }
        public string FormattedDate
        {
            get
            {
                if (Created_date.HasValue)
                {
                    return Created_date.Value.ToString("MM/dd/yyyy hh:mm tt");
                }
                return string.Empty; // Or any other default value you prefer
            }
        }
    }
}
