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

        //public List<ExportExcelSheetData_Dto> GetUserDetails(string )
        //{


        //    return UserData;
        //}


        public static string FileName(string Filename)
        {
            string[] parts = Filename.Split('_');
            string filename = parts.Length >= 2 ? parts[1] : Filename;

            return filename;
        }

        public List<ExportExcelSheetData_Dto> GetUserDetails(string ID)
        {
            throw new NotImplementedException();
        }

        public void InsertGSTBillsDetails(GSTBills_Dto gstBillsData)
        {

            GSTBillsData Record = new GSTBillsData();

            Record.UserID = gstBillsData.UserID;
            Record.SessionID = gstBillsData.SessionID;
            Record.FileID = gstBillsData.FileID;
            Record.Tax = gstBillsData.Tax;
            Record.Interest = gstBillsData.Interest;
            Record.penalty = gstBillsData.penalty;
            Record.fees = gstBillsData.fees;
            Record.total = gstBillsData.total;
            Record.PaymentMode = gstBillsData.PaymentMode;
            Record.CreatedDate=DateTime.Now;


            _context.GSTBills.Add(Record);
            _context.SaveChanges();

        }
    }
}