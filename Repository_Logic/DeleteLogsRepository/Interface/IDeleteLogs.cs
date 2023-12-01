using Repository_Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.DeleteLogsRepository.Interface
{
    public interface IDeleteLogs
    {

        public void Insert(DeleteLog_Dto deleteLog_Dto);

        public List<DeleteLog_Dto> GetAllDeleteLogs();
        public List<DeleteLog_Dto> ViewDeleteLogsDataTable(DataTable_Dto dataTable_);
    }
}
