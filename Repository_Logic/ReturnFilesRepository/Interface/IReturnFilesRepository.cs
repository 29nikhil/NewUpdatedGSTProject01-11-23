using Repository_Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.ReturnFilesRepository.Interface
{
    public interface IReturnFilesRepository
    {

        public Task<List<File_Details_Excel_Dto>> GetReturnedFilesData();

        //public string FileName(string Filename);
        public Task<List<File_Details_Excel_Dto>> GetReturnedFilesDataForFellowship(string UserID);
        public Task<List<File_Details_Excel_Dto>> ViewReturnFilesDataTable(DataTable_Dto dataTable, string LoginSessionID);
    }
}
