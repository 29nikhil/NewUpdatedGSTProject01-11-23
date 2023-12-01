using Repository_Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.QueryRepository.Interface
{
    public interface IQuery
    {
        public void insert(Query_Dto query_Dto);

        public Task<List<Query_Dto>> GetAllQueries(string LoginSessionID);

        public void InsertAnswerToQuestion(Query_Dto query_Dto, string LoginSessionID);

        public Task<List<Query_Dto>> QueryListForUserDatatable(DataTable_Dto dataTable_, string LoginSessionID);
        public Task<List<Query_Dto>> QueryListForCADatatable(DataTable_Dto dataTable_, string LoginSessionID);
    }
}
