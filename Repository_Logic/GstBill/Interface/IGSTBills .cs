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
      
        public void InsertGSTBillsDetails(GSTBills_Dto gstBillsData,string LoginSessionID);

        public List<GSTBills_Dto> GetGSTBillsDetails();
        public List<GSTBills_Dto> ShowGSTBillsDatatable(DataTable_Dto dataTable, string LoginSessionID);
    }
}
