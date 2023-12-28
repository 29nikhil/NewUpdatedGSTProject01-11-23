using Repository_Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.ErrorLogsRepository.Interface
{
    public  interface IErrorLogs
    {

        public void InsertErrorLog(ErrorLog_Dto errorLog);
        public  Task SendErrorDetailsThroughEmail(ErrorLog_Dto errorDetails);

      


    }
}
