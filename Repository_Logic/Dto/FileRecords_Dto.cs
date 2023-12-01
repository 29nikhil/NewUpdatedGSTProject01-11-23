using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
    public class FileRecords_Dto
    {

        public string id { get; set; }
        public string? FileId { get; set; }
        public string? ProductName { get; set; }
        public string? HSE_SAC_Code { get; set; }
        public string? Qty { get; set; }
        public string? Rate { get; set; } //State GST + Central GST
        public string? Ammount { get; set; }
        public string? Disc { get; set; }
        public string? TaxableValue { get; set; }

        public string? SC_GST_Rate { get; set; }
        public string? SC_GST_Ammount { get; set; }

        public string? Total { get; set; }
        public string? NetAmmount { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;

        public DateTime? Modified_date { get; set; }
        public bool? IsDeleted { get; set; }

        public bool? IsTestdata { get; set; }

        [NotMapped]
        public string? Remark { get; set; }
        [NotMapped]
        public string ExtractedDate { get; set; }
    }
}
