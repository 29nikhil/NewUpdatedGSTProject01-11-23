using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class ExcelSheetData
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public string? name { get; set; }
        public string? no { get; set; }
        public string? Add { get; set; }
        public string? UserID { get; set; }
        public string? GSTType { get; set; }
        public string? status { get; set; }
        public string? UniqueFileId { get; set; }
        public DateTime Date { get; set; }
    
        public string? SessionID { get; set; } //Uploadby User Session Id CA||Fellowship

        [NotMapped]
        public string Remark { get; set; }
        [NotMapped]
        public string ExtractedDate { get; set; }
    }
}
