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
    public class ReturnFiles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string ID { get; set; }
        public string? UserID { get; set; }
        public string? CA_ID { get; set; }

        public string? UplodedById { get; set; }
        public string? FileID { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

        public bool? IsTestData { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
