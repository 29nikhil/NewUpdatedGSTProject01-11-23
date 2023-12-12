using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
    public class GSTBills_Dto
    {
        public string? ID { get; set; }
        public string UserID { get; set; }

        public string FileUploadedBy { get; set; }
         public string? GSTBillSubmittedBy { get; set; }
        public string UserName { get; set; }
        public string GSTType { get; set; }
        public string FileName { get; set; }
        public string GSTNo { get; set; }
        public string Email { get; set; }
        public string OrganisationType { get; set; }
        public string FileID { get; set; }
        public string Tax { get; set; }
        public string Interest { get; set; }
        public string penalty { get; set; }
        public string fees { get; set; }
        public Double? total { get; set; }
        public string PaymentMode { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string Date { get;set; }
        public DateTime ModifiedDate { get; set; }

        public bool IsTestData { get; set; }

        public bool IsDeleted { get; set; }


    }
}
