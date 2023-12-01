using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
    public class File_Details_Excel_Dto
    {

          

            public string FileId { get; set; }
           public string FileName { get; set; }

        public string? UserId { get; set; }
            public string? UserName {get; set; }
            public string? GstNo { get; set; }
            public string? Email { get; set; }
            public string? UplodedById { get; set; }
            public string? UplodedByName { get; set; }
            public string? CA_ID { get; set; }
            public string? CA_Name { get; set; }
            public string? BusinessType { get; set; }
            public string? GSTType { get; set; }
            public int ? Total {  get; set; }
            public string? Status { get; set; }
            public DateTime? Date { get; set; } = DateTime.Now;

            public DateTime? Modified_date { get; set; }
            public bool? IsDeleted { get; set; }

            public bool? IsTestdata { get; set; }
        }
}
