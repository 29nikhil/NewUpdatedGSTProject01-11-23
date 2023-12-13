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

        public string? FileUploadedBy { get; set; }
        public string? GSTBillSubmittedBy { get; set; }
        public string? FileID { get; set; }
        public Double? Tax { get; set; }
        public Double? Interest { get; set; }
        public Double? penalty { get; set; }
        public Double? fees { get; set; }
        public Double? total { get; set; }
        public string? PaymentMode { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        [NotMapped]
        public string FormattedDate
        {
            get
            {
                if (CreatedDate.HasValue)
                {
                    return CreatedDate.Value.ToString("MM/dd/yyyy hh:mm tt");
                }
                return string.Empty; // Or any other default value you prefer
            }
        }
        public DateTime? ModifiedDate { get; set; }

        public bool? IsTestData { get; set; }

        public bool? IsDeleted { get; set; }



    }
}