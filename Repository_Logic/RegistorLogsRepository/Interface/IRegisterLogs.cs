using Microsoft.AspNetCore.Http;
using Repository_Logic.Dto;
using Repository_Logic.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.RegistorLogsRepository.Interface
{
    public interface IRegisterLogs
    {
        public Task<string> SaveRegistorLogsUser(string userId,string RegistorById);
        public Task<string> SaveRegistorLogsFellowship(string userId, string RegistorById);
        public IEnumerable<RegisterLogVIew_Dto> GetAllRegistorLogs();
        public  Task<List<RegisterLogVIew_Dto>> ViewRegisterLogsDataTable(DataTable_Dto dataTable_);


    }
}
