using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Http;
using Repository_Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.ExportExcelSheet.Interface
{
    public interface IExportExcelSheet
    {
        public void Insert(Excel_Dto ExcelData);
        public List<ExcelSheetData> GetDataByFileID(string FileID);
        public string UpdateExcelSheetRecord(Excel_Dto ExcelSheetRecord);
        public void updateStatusFieldOfFileData(List<ExcelSheetData> excelSheetData, string StatusToBeUpdate);

        public void InsertNewExcelSheetRecord(Excel_Dto ExcelSheetRecord);

      
        public Task<string> ExportExcelSheetData(IFormFile file, string hiddenInput, string GSTType, string userId);
        public UserOtherDetailsForExport_Dto UserOtherDetailsForExport(string ID);

        public Task<List<ExportExcelSheetData_Dto>> GetUserDataForExcelSheet(string UserName);

        public Task<List<ExportExcelSheetData_Dto>> ExcelSheetDatatable(DataTable_Dto dataTable, string LoginSessionID);

        public List<ExcelSheetData> ExportExcelSheetRecordsDatatable(DataTable_Dto dataTable, string id);
    }
}
