using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Repository_Logic.Dto;
using Repository_Logic.GstBill.Interface;
using Repository_Logic.UserOtherDatails.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repository_Logic.GstBill.Implementation
{
    public class GSTBills : IGSTBills
    {

        private readonly Application_Db_Context _context;
        private Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;
        private readonly IExtraDetails _extraDetails;

        public GSTBills(Application_Db_Context context, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> IdentityUserManager)
        {
            _IdentityUserManager = IdentityUserManager;
            _context = context;
        }

        public static string FileName(string Filename)
        {
            string[] parts = Filename.Split('_');
            string filename = parts.Length >= 2 ? parts[1] : Filename;

            return filename;
        }



        public void InsertGSTBillsDetails(GSTBills_Dto gstBillsData, string LoginSessionID)
        {

            GSTBillsData Record = new GSTBillsData();

            Record.UserID = gstBillsData.UserID;
            Record.FileUploadedBy = gstBillsData.FileUploadedBy;
            Record.GSTBillSubmittedBy = LoginSessionID;
            Record.FileID = gstBillsData.FileID;
            Record.Tax = Double.Parse(gstBillsData.Tax);
            Record.Interest = Double.Parse(gstBillsData.Interest);
            Record.penalty = Double.Parse(gstBillsData.penalty);
            Record.fees = Double.Parse(gstBillsData.fees);

            Record.total = gstBillsData.total;
            Record.PaymentMode = gstBillsData.PaymentMode;
            Record.CreatedDate = DateTime.Now;


            _context.GSTBills.Add(Record);
            _context.SaveChanges();

        }

        public List<GSTBills_Dto> GetGSTBillsDetails()
        {
            var GSTBillsRecords = _context.GSTBills.Select(data =>
             new GSTBills_Dto
             {
                 UserID = _IdentityUserManager.Users.Where(x => x.Id == data.UserID).Select(x => x.UserName).FirstOrDefault(),
                 FileUploadedBy = _IdentityUserManager.Users.Where(x => x.Id == data.FileUploadedBy).Select(x => x.UserName).FirstOrDefault(),
                 GSTBillSubmittedBy = _IdentityUserManager.Users.Where(x => x.Id == data.GSTBillSubmittedBy).Select(x => x.UserName).FirstOrDefault(),
                 FileID = FileName(data.FileID),
                 Tax = data.Tax.ToString(),
                 Interest = data.Interest.ToString(),
                 penalty = data.penalty.ToString(),
                 fees = data.fees.ToString(),
                 total = data.total,
                 PaymentMode = data.PaymentMode,
                 Date = data.CreatedDate.Value.ToString("MM/dd/yyyy hh:mm tt")

             }).ToList();
            return GSTBillsRecords;

        }

        public List<GSTBills_Dto> ShowGSTBillsDatatable(DataTable_Dto dataTable, string LoginSessionID)
        {
            List<GSTBills_Dto> GSTBillsData = GetGSTBillsDetails();

            if (!string.IsNullOrEmpty(dataTable.SearchValue))
            {
                string searchValue = dataTable.SearchValue.ToLower();
                GSTBillsData = GSTBillsData.Where(x =>
                    x.UserID.ToLower().Contains(searchValue) ||
                    x.FileUploadedBy.ToLower().Contains(searchValue) ||
                    x.FileID.ToLower().Contains(searchValue) ||
                    x.Tax.Contains(searchValue) ||
                    x.Interest.Contains(searchValue) ||
                    x.penalty.Contains(searchValue) ||
                    x.fees.Contains(searchValue) ||
                    x.total.ToString().Contains(searchValue) ||
                    x.PaymentMode.Contains(searchValue) ||
                    x.Date.Contains(searchValue)
                ).ToList();
            }

            if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDirection))
            {
                switch (dataTable.sortColumn)
                {
                    case "UserID":

                        GSTBillsData = dataTable.sortColumnDirection == "asc" ?
                GSTBillsData.OrderBy(u => u.UserID).ToList() :
                GSTBillsData.OrderByDescending(u => u.UserID).ToList();
                        break;
                    case "SessionID":

                        GSTBillsData = dataTable.sortColumnDirection == "asc" ? GSTBillsData.OrderBy(u => u.FileUploadedBy).ToList() : GSTBillsData.OrderByDescending(u => u.FileUploadedBy).ToList();
                        break;
                    case "FileID":
                        GSTBillsData = dataTable.sortColumnDirection == "asc" ? GSTBillsData.OrderBy(u => u.FileID).ToList() : GSTBillsData.OrderByDescending(u => u.FileID).ToList();
                        break;
                    case "Tax":
                        GSTBillsData = dataTable.sortColumnDirection == "asc" ? GSTBillsData.OrderBy(u => u.Tax).ToList() : GSTBillsData.OrderByDescending(u => u.Tax).ToList();
                        break;
                    case "Interest":
                        GSTBillsData = dataTable.sortColumnDirection == "asc" ? GSTBillsData.OrderBy(u => u.Interest).ToList() : GSTBillsData.OrderByDescending(u => u.Interest).ToList();
                        break;
                    case "penalty":
                        GSTBillsData = dataTable.sortColumnDirection == "asc" ? GSTBillsData.OrderBy(u => u.penalty).ToList() : GSTBillsData.OrderByDescending(u => u.penalty).ToList();
                        break;
                    case "fees":
                        GSTBillsData = dataTable.sortColumnDirection == "asc" ? GSTBillsData.OrderBy(u => u.fees).ToList() : GSTBillsData.OrderByDescending(u => u.fees).ToList();
                        break;
                    case "total":
                        GSTBillsData = dataTable.sortColumnDirection == "asc" ? GSTBillsData.OrderBy(u => u.total).ToList() : GSTBillsData.OrderByDescending(u => u.total).ToList();
                        break;
                    case "PaymentMode":
                        GSTBillsData = dataTable.sortColumnDirection == "asc" ? GSTBillsData.OrderBy(u => u.PaymentMode).ToList() : GSTBillsData.OrderByDescending(u => u.PaymentMode).ToList();
                        break;
                    case "Date":
                        GSTBillsData = dataTable.sortColumnDirection == "asc" ? GSTBillsData.OrderBy(u => u.Date).ToList() : GSTBillsData.OrderByDescending(u => u.Date).ToList();
                        break;

                }
            }


            return GSTBillsData;


        }
    }
}