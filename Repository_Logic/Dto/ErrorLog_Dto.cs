using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
    public class ErrorLog_Dto
    {
       
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

    }
}
