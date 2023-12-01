using Repository_Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.GstBill.Interface
{
    public interface IGSTBills
    {
        public List<ExportExcelSheetData_Dto> GetUserDetails(string ID);

        public void InsertGSTBillsDetails(GSTBills_Dto gstBillsData);
    }
}
