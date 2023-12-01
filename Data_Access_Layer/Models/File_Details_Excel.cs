using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class File_Details_Excel
    {

        [Key]

        public string FileId { get; set; }
        public string? UserId { get; set; }
        public string? UplodedById { get; set; }
        public string? CA_ID { get; set; }
        public string? GSTTye { get; set; }
        
        public string? Status { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;

        public DateTime? Modified_date { get; set; }
        public bool? IsDeleted { get; set; }

        public bool? IsTestdata { get; set; }

    }
}
