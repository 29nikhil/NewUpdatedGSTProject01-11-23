using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
    public class ViewFilleGSt_Dto
    {
        public string? ID { get; set; }
        public string UserID { get; set; }

        public string SessionID { get; set; }
        public string FiledBy { get; set; }
        public string TaxPeriod {get; set; }
        public string FileID { get; set; }
        public string GSTType { get; set; }
        public string GSTNo { get; set; }
        public string Email { get; set; }
        public string BusinessType { get; set; }

        public string FileName { get; set; }
        public string SessionName { get; set; }
        public string Status { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public long total { get; set; }
        public string PaymentMode { get; set; }
        public string CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public bool IsTestData { get; set; }

        public bool IsDeleted { get; set; }



    }
}
