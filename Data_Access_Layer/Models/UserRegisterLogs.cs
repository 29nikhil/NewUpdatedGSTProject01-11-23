using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Data_Access_Layer.Models
{
    public  class UserRegisterLogs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string Id { get; set; }
        public string? UserID { get; set; }
        public string? CA_ID { get; set; }
       
        public string? RegistorById { get; set; }
        public string? RegistorByUserName { get; set; }
        public string? RegistorByEmail { get; set; }
        public string? CA_Email { get; set; }
        public string? UserRole { get; set; }

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
