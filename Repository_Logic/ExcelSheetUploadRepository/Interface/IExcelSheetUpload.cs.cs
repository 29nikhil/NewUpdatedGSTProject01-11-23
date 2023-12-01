using Microsoft.AspNetCore.Http;
using Repository_Logic.Dto;
using Repository_Logic.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.ExcelSheetUploadRepository.Interface
{
    public interface IExcelSheetUpload
    {
        public void InsertNewExcelSheetRecord(FileRecords_Dto ExcelSheetRecord);
        public string UpdateExcelSheetRecord(FileRecords_Dto ExcelData);

        public UserOtherDetailsForExport_Dto UserOtherDetailsForExport(string ID);


        public Task<string> ExportExcelSheetData(IFormFile file, string UplodedById, string GSTType, string userId );
        public void Insert(FileRecords_Dto ExcelData);

        public  Task<List<File_Details_Excel_Dto>> GetUserDataForExcelSheet(string LoginSessionID);
        public  Task<List<File_Details_Excel_Dto>> ExcelSheetDatatable(DataTable_Dto dataTable, string LoginSessionID);
        public List<FileRecords_Dto> GetDataByFileID(string FileID);
        public List<FileRecords_Dto> ExportExcelSheetRecordsDatatable(DataTable_Dto dataTable, string id);

        public void UpdateStatus(string FileID, string StatusToBeUpdate);


    }
}
