using Data_Access_Layer.Models;
using Repository_Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.LoginLogsDataRepository.Interface
{
    public interface ILoginLogs
    {

        public void insert(LoginLogs_Dto loginDetails);

        public List<LoginLogs_Dto> GetLoginLogs();

        public List<LoginLogs_Dto> ViewLoginLogsDataTable(DataTable_Dto dataTable_);

    }
}
