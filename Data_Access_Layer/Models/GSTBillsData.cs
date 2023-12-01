using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class GSTBillsData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string ID { get; set; }
        public string? UserID { get; set; }

        public string? SessionID { get; set; }
        public string? FileID { get; set; }
        public long? Tax { get; set; }
        public long? Interest { get; set; }
        public long? penalty { get; set; }
        public long? fees { get; set; }
        public long? total { get; set; }
        public string? PaymentMode { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

        public bool? IsTestData { get; set; }

        public bool? IsDeleted { get; set; }



    }
}
