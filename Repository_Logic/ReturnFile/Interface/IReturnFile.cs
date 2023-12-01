using Data_Access_Layer.Models;
using Repository_Logic.Dto;
using Repository_Logic.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.ReturnFile.Interface
{
    public interface IReturnFile
    {
        public Task<List<ExportExcelSheetData_Dto>> GetReturnedFilesData();

        public string FileName(string Filename);
        public Task<List<ExportExcelSheetData_Dto>> GetReturnedFilesDataForFellowship(string UserID);
        public Task<List<ExportExcelSheetData_Dto>> ViewReturnFilesDataTable(DataTable_Dto dataTable, string LoginSessionID);

      





    }
}
