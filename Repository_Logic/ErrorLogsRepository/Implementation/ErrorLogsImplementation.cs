using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Repository_Logic.Dto;
using Repository_Logic.ErrorLogsRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.ErrorLogsRepository.Implementation
{
    public class ErrorLogsImplementation : IErrorLogs
    {

        private readonly Application_Db_Context _context;
        public ErrorLogsImplementation(Application_Db_Context context)
        {
            _context = context;
        }
        public void InsertErrorLog(ErrorLog_Dto errorLog)
        {
            ErrorLog errorDetails = new ErrorLog();
            errorDetails.Date = DateTime.Now;
            errorDetails.Message = errorLog.Message;
            errorDetails.StackTrace = errorLog.StackTrace;
            _context.errorLogs.Add(errorDetails);
            _context.SaveChanges();
        }
    }
}
