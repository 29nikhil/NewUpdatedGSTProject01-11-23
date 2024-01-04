using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class LoginLogs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string Id { get; set; }
        public string? UserID { get; set; }
        public string? Message { get; set; }
        public string? StatusLogin { get; set; }
        public string? CurrentStatus { get; set; } //Current Log in or not status check
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        [NotMapped]
        public string FormattedDate
        {
            get
            {
                if (CreatedDate.HasValue)
                {
                    return CreatedDate.Value.ToString("dd/MM/yyyy hh:mm tt");
                }
                return string.Empty; // Or any other default value you prefer
            }
        }
        public bool? IsTestData { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
