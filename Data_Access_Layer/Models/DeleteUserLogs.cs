using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class DeleteUserLogs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string? UserID { get; set; }

        public string? DeletedById { get; set; }
        public string? UserName { get; set; }
        public string? DeletedByName { get; set; }

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
