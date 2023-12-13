using Data_Access_Layer.Db_Context;
using Microsoft.EntityFrameworkCore;
using Repository_Logic.RecoverUser.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.RecoverUser.Implementation
{
    public class recoverUser : IRecoverUser
    {
        private readonly Application_Db_Context _context;

        public recoverUser(Application_Db_Context context)
        {
            _context = context;
        }

        public void recover(string id)
        {

            var DeleteLogRecord = _context.DeleteUserLogs.Where(model => model.Id == id).FirstOrDefault();
            var UserId = DeleteLogRecord.UserID;
            _context.Entry(DeleteLogRecord).State = EntityState.Deleted;
            _context.SaveChanges();

            var UserRecord = _context.appUser.Where(model => model.Id == UserId).FirstOrDefault();
            UserRecord.IsDeleted = false;

            _context.Entry(UserRecord).State = EntityState.Modified;
            _context.SaveChanges();



        }
    }
}
