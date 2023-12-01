using Repository_Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.ViewFilledGSt.Interface
{
    public interface IViewGSTFilledGst
    {


        public Task<List<ViewFilleGSt_Dto>> GetReturnedFilesDataForUser(string UserID);
        public Task<List<ViewFilleGSt_Dto>> ViewReturnFilesDataTableUser(DataTable_Dto dataTable, string UserId);        
        


    }
}
