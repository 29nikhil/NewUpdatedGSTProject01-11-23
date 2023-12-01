using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class AllocatedTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }

        public string SessionID { get; set; }

        public string userID { get; set; }
        public string FileID { get; set; }

        public string Remark { get; set; }

        public string Login_SessionID { get; set; }

        public string status { get; set; }

        public DateTime Created_date { get; set; } = DateTime.Now;
        public DateTime Modified_date { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsTestdata { get; set; }

    }

}
