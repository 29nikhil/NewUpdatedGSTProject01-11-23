using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Repository_Logic.DeleteLogsRepository.Interface;
using Repository_Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.DeleteLogsRepository.Implementation
{
    public class DeleteLog : IDeleteLogs
    {
        private readonly Application_Db_Context _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;

        public DeleteLog(Application_Db_Context context, UserManager<IdentityUser> identityUserManager)
        {
            _context = context;
            _IdentityUserManager = identityUserManager;
        }

        public void Insert(DeleteLog_Dto deleteLog_Dto)
        {
            DeleteUserLogs deleteUserLogs = new DeleteUserLogs();

            deleteUserLogs.Id = deleteLog_Dto.Id;
            deleteUserLogs.UserID = deleteLog_Dto.UserID;
            deleteUserLogs.DeletedById = deleteLog_Dto.DeletedById;
            deleteUserLogs.UserName = deleteLog_Dto.UserName;
            deleteUserLogs.DeletedByName = deleteLog_Dto.DeletedByName;
            _context.DeleteUserLogs.Add(deleteUserLogs);
            _context.SaveChanges();

        }

        public List<DeleteLog_Dto> GetAllDeleteLogs()
        {
            List<DeleteLog_Dto> deletelogsdetails = _context.DeleteUserLogs.Select(

                deletelogs => new DeleteLog_Dto
                {
                    Id = deletelogs.Id,
                    UserID = deletelogs.UserID,
                    DeletedById = deletelogs.DeletedById,
                    UserName = deletelogs.UserName,
                    DeletedByName = deletelogs.DeletedByName,
                    CreatedDate = deletelogs.CreatedDate,
                    Date = deletelogs.FormattedDate,

                }).ToList();
            return deletelogsdetails;
        }

        public List<DeleteLog_Dto> ViewDeleteLogsDataTable(DataTable_Dto dataTable_)
        {
            var Records = GetAllDeleteLogs();


            if (!string.IsNullOrEmpty(dataTable_.SearchValue))
            {
                Records = Records.Where(x => x.UserName.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.DeletedByName.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.Date.ToLower().Contains(dataTable_.SearchValue.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(dataTable_.sortColumn) && !string.IsNullOrEmpty(dataTable_.sortColumnDirection))
            {

                IEnumerable<DeleteLog_Dto> Deletelog = Records;
                switch (dataTable_.sortColumn)
                {
                    case "UserName":
                        Deletelog = dataTable_.sortColumnDirection == "asc" ?
                        Deletelog.OrderBy(u => u.UserName) : Deletelog.OrderByDescending(u => u.UserName);
                        break;
                    case "DeletedByName":
                        Deletelog = dataTable_.sortColumnDirection == "asc" ?
                        Deletelog.OrderBy(u => u.DeletedByName) : Deletelog.OrderByDescending(u => u.DeletedByName);
                        break;
                    case "Date":
                        Deletelog = dataTable_.sortColumnDirection == "asc" ?
                        Deletelog.OrderBy(u => u.Date) : Deletelog.OrderByDescending(u => u.Date);
                        break;
                }

                Records = Deletelog.ToList();

            }

            return Records;
        }
    }
}
