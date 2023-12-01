using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
    public class ExportExcelSheetData_Dto
    {

        public string userId { get; set; }
        public string UniqueFileId { get; set; }
        public string UserName { get; set; }
        public string GSTType { get; set; }
        public string GSTNo { get; set; }
        public string Email { get; set; }
        public string OrganisationType { get; set; }
        public string FileName { get; set; }
        public string SessionID { get; set; }
        public string SessionName { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

    }
}
