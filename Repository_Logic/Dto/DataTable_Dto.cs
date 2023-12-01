using Azure.Core;
using Repository_Logic.UserOtherDatails.implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
    public class DataTable_Dto
    {
        public int TotalRecord { get; set; }
        public int FilterRecord { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public string Draw { get; set; }
        public string SearchValue { get; set; }
        public string sortColumn { get; set; }
        public string sortColumnDirection { get; set; }

        //int totalRecord = 0;
        //int filterRecord = 0;
        //var draw = Request.Form["draw"].FirstOrDefault();
        
    }
}
