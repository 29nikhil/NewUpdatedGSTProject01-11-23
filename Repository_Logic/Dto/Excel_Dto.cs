using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
    public class Excel_Dto
    {
        public string id { get; set; }
        public string? name { get; set; }
        public string? no { get; set; }
        public string? Add { get; set; }
        public string? UserID { get; set; }
        public string? GSTType { get; set; }
        public string? status { get; set; }
        public string? UniqueFileId { get; set; }
        public DateTime Date { get; set; }
        public string? SessionID { get; set; }

        [NotMapped]
        public string Remark { get; set; }

    }
}
