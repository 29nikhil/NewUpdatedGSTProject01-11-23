using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository_Logic.Dto;
using Repository_Logic.ExcelSheetUploadRepository.Interface;
using Repository_Logic.ExportExcelSheet.Interface;
using Repository_Logic.TaskAllocation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.TaskAllocation.Implementation
{
    public class TaskAllocation : ITaskAllocation
    {
        private readonly Application_Db_Context _context;
        private readonly IExportExcelSheet _exportExcelSheet;
        private Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _IdentityUserManager;
        private IExcelSheetUpload _excelSheetUpload;
        public TaskAllocation(Application_Db_Context context, IExportExcelSheet exportExcelSheet, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> IdentityUserManager, IExcelSheetUpload excelSheetUpload)
        {
            _context = context;
            _exportExcelSheet = exportExcelSheet;
            _IdentityUserManager = IdentityUserManager;
            _excelSheetUpload= excelSheetUpload;
        }

        public void ChangesDone(string Id)
        {

            var Record = _context.AllocatedTasks.Where(p => p.ID == Id).FirstOrDefault();
            Record.status = "Changes Done";
            _context.Entry(Record).State = EntityState.Modified;
            _context.SaveChanges();

            var ChangesPendingListOfGivenFile = _context.AllocatedTasks.Where(p => p.FileID == Record.FileID && p.status == "Changes Pending").ToList();
            var count = ChangesPendingListOfGivenFile.Count;
            if (count == 0)
            {
                var FileDetails = _exportExcelSheet.GetDataByFileID(Record.FileID);
                var StatusToBeUpdate = "Changes Done";
                _exportExcelSheet.updateStatusFieldOfFileData(FileDetails, StatusToBeUpdate);
            }




        }
        public void ChangesDoneTask(string Id)
        {

            var Record = _context.TaskAllocated.Where(p => p.ID == Id).FirstOrDefault();
            Record.status = "Changes Done";
            _context.Entry(Record).State = EntityState.Modified;
            _context.SaveChanges();

            var ChangesPendingListOfGivenFile = _context.TaskAllocated.Where(p => p.FileID == Record.FileID && p.status == "Changes Pending").ToList().Count();
            var count = ChangesPendingListOfGivenFile;
            if (count <= 0)
            {
                var FileDetails = _exportExcelSheet.GetDataByFileID(Record.FileID);
                var StatusToBeUpdate = "Changes Done";
                _excelSheetUpload.UpdateStatus(Record.FileID, StatusToBeUpdate);

            }
            else
            {

            }


        }
        public List<TaskList_Dto> GetDataBySessionID(string SessionID)
        {
            var Records = _context.AllocatedTasks.OrderByDescending(p => p.Created_date).Select(data =>

            new TaskList_Dto
            {
                Id = data.ID,
                FileName = TaskAllocation.FileName(data.FileID),
                UserName = _IdentityUserManager.Users.Where(x => x.Id == data.userID).Select(x => x.UserName).FirstOrDefault(),
                userId = data.userID,
                CAName = _IdentityUserManager.Users.Where(x => x.Id == data.Login_SessionID).Select(x => x.UserName).FirstOrDefault(),
                SessionID = _IdentityUserManager.Users.Where(x => x.Id == data.SessionID).Select(x => x.UserName).FirstOrDefault(),
                Remark = data.Remark,
                status = data.status,
                uniqueFileId = data.FileID,
                Created_Date = data.Created_date,
                date = data.Created_date.Date.ToString("yyyy-MM-dd")
            }).ToList();
            return Records;
        }




        public async Task<List<TaskView_Dto>> TaskListAsync(string SessionID) //new Update 0.1
        {
            var user = await _IdentityUserManager.FindByIdAsync(SessionID);
            var isInRole = await _IdentityUserManager.IsInRoleAsync(user, "Fellowship");
            List<TaskView_Dto> UserData = new List<TaskView_Dto>();

            if (isInRole)
            {


                UserData = (
                  from T1 in _context.appUser
                  join T3 in _context.TaskAllocated on T1.Id equals T3.userID
                  where (T3.status == "Changes Pending" ) && T3.AllocatedById == SessionID


                  select new TaskView_Dto
                  {  ID=T3.ID,
                      FileID = T3.FileID,
                      FileName = FileName(T3.FileID),
                      status = T3.status,
                      AllocatedById = T3.AllocatedById,
                      AllocatedByName = _IdentityUserManager.Users.Where(x => x.Id == T3.AllocatedById).Select(x => x.UserName).FirstOrDefault(),
                      CAName = _IdentityUserManager.Users.Where(x => x.Id == T3.CA_ID).Select(x => x.UserName).FirstOrDefault(),
                      userID = T3.userID,
                      Remark=T3.Remark,
                      UserName = T1.UserName,                    
                      CA_ID = T3.CA_ID,
                      Created_date=T3.FormattedDate

                  }).ToList();



            }
            else
            {
               
                UserData = (
                  from T1 in _context.appUser
                  join T3 in _context.TaskAllocated on T1.Id equals T3.userID
                


                  select new TaskView_Dto
                  {
                      ID = T3.ID,
                      FileID = T3.FileID,
                      FileName = FileName(T3.FileID),
                      status = T3.status,
                      AllocatedById = T3.AllocatedById,
                      AllocatedByName = _IdentityUserManager.Users.Where(x => x.Id == T3.AllocatedById).Select(x => x.UserName).FirstOrDefault(),
                      CAName = _IdentityUserManager.Users.Where(x => x.Id == T3.CA_ID).Select(x => x.UserName).FirstOrDefault(),
                      userID = T3.userID,
                      Remark = T3.Remark,
                      UserName = T1.UserName,
                      CA_ID = T3.CA_ID,
                      Created_date = T3.FormattedDate

                  }).ToList();



            }
         
            return UserData;
        }

        public List<TaskList_Dto> GetDataBySessionIDForFellowship(string SessionID)
        {
            var Records = _context.AllocatedTasks.Where(p => p.SessionID == SessionID).OrderByDescending(p => p.Created_date).Select(data =>
              new TaskList_Dto
              {
                  Id = data.ID,
                  FileName = TaskAllocation.FileName(data.FileID),
                  UserName = _IdentityUserManager.Users.Where(x => x.Id == data.userID).Select(x => x.UserName).FirstOrDefault(),
                  userId = data.userID,
                  CAName = _IdentityUserManager.Users.Where(x => x.Id == data.Login_SessionID).Select(x => x.UserName).FirstOrDefault(),
                  SessionID = _IdentityUserManager.Users.Where(x => x.Id == data.SessionID).Select(x => x.UserName).FirstOrDefault(),
                  Remark = data.Remark,
                  status = data.status,
                  uniqueFileId = data.FileID,
                  Created_Date = data.Created_date,
                  date = data.Created_date.Date.ToString("yyyy-MM-dd")
              }

            ).ToList();

            return Records;
        }


        public void Insert(AllocatedTask_Dto allocatedTask_Dto)
        {
            AllocatedTask allocatedTask = new AllocatedTask();
            allocatedTask.Remark = allocatedTask_Dto.Remark;
            allocatedTask.status = allocatedTask_Dto.status;
            allocatedTask.SessionID = allocatedTask_Dto.SessionID;
            allocatedTask.Login_SessionID = allocatedTask_Dto.Login_SessionID;
            allocatedTask.FileID = allocatedTask_Dto.FileID;
            allocatedTask.userID = allocatedTask_Dto.userID;
            allocatedTask.Created_date = allocatedTask_Dto.Created_date;
            _context.AllocatedTasks.Add(allocatedTask);
            _context.SaveChanges();

        }




        public void InsertTask(TaskAllowcated_Dto allocatedTask_Dto)// new Update 0.1
        {
            TaskAllocated allocatedTask = new TaskAllocated();
            allocatedTask.Remark = allocatedTask_Dto.Remark;
            allocatedTask.status = allocatedTask_Dto.status;
            allocatedTask.AllocatedById = allocatedTask_Dto.AllocatedById;
            allocatedTask.CA_ID = allocatedTask_Dto.CA_ID;
            allocatedTask.FileID = allocatedTask_Dto.FileID;
            allocatedTask.userID = allocatedTask_Dto.userID;
            allocatedTask.Created_date = allocatedTask_Dto.Created_date;
            _context.TaskAllocated.Add(allocatedTask);
            _context.SaveChanges();

        }
        public static string FileName(string Filename)
        {
            string[] parts = Filename.Split('_');
            string filename = parts.Length >= 2 ? parts[1] : Filename;

            return filename;
        }

        public async Task<List<TaskList_Dto>> ViewTaskDataTable(DataTable_Dto dataTable_, string LoginSessionID)
        {
            var user = await _IdentityUserManager.FindByIdAsync(LoginSessionID);
            var isInRole = await _IdentityUserManager.IsInRoleAsync(user, "Fellowship");
            var Records = new List<TaskList_Dto>();
            if (isInRole)
            {
                Records = GetDataBySessionIDForFellowship(LoginSessionID);

            }
            else
            {

                Records = GetDataBySessionID(LoginSessionID);
            }
            if (!string.IsNullOrEmpty(dataTable_.SearchValue))
            {
                Records = Records.Where(x => x.FileName.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.UserName.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.CAName.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.SessionID.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.Remark.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.status.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.uniqueFileId.ToLower().Contains(dataTable_.SearchValue.ToLower())|| (x.date.Contains(dataTable_.SearchValue))).ToList();
            }

            if (!string.IsNullOrEmpty(dataTable_.sortColumn) && !string.IsNullOrEmpty(dataTable_.sortColumnDirection))
            {

                IEnumerable<TaskList_Dto> TaskDetails = Records;

                switch (dataTable_.sortColumn)
                {

                    case "FileName":
                        TaskDetails = dataTable_.sortColumnDirection == "asc" ?
                                TaskDetails.OrderBy(u => u.FileName) : TaskDetails.OrderByDescending(u => u.FileName);
                        break;

                    case "UserName":
                        TaskDetails = dataTable_.sortColumnDirection == "asc" ?
                                TaskDetails.OrderBy(u => u.UserName) : TaskDetails.OrderByDescending(u => u.UserName);
                        break;

                    case "CAName":
                        TaskDetails = dataTable_.sortColumnDirection == "asc" ?
                                TaskDetails.OrderBy(u => u.CAName) : TaskDetails.OrderByDescending(u => u.CAName);
                        break;

                    case "SessionID":
                        TaskDetails = dataTable_.sortColumnDirection == "asc" ?
                                TaskDetails.OrderBy(u => u.SessionID) : TaskDetails.OrderByDescending(u => u.SessionID);
                        break;
                    case "Remark":
                        TaskDetails = dataTable_.sortColumnDirection == "asc" ?
                                TaskDetails.OrderBy(u => u.Remark) : TaskDetails.OrderByDescending(u => u.Remark);
                        break;
                    case "status":
                        TaskDetails = dataTable_.sortColumnDirection == "asc" ?
                            TaskDetails.OrderBy(u => u.status) : TaskDetails.OrderByDescending(u => u.status);
                        break;

                    case "uniqueFileId":
                        TaskDetails = dataTable_.sortColumnDirection == "asc" ?
                                TaskDetails.OrderBy(u => u.uniqueFileId) : TaskDetails.OrderByDescending(u => u.uniqueFileId);
                        break;
                    case "date":
                        TaskDetails = dataTable_.sortColumnDirection == "asc" ?
                                TaskDetails.OrderBy(u => u.date) : TaskDetails.OrderByDescending(u => u.date);
                        break;

                }



                Records = TaskDetails.ToList();

            }

            return Records;
        }





        public async Task<List<TaskView_Dto>> ViewTaskListDataTable(DataTable_Dto dataTable_, string LoginSessionID) //New Update 0.1
        {
            var user = await _IdentityUserManager.FindByIdAsync(LoginSessionID);
            var isInRole = await _IdentityUserManager.IsInRoleAsync(user, "Fellowship");
            var Records = new List<TaskView_Dto>();
           

                Records = await TaskListAsync(LoginSessionID);
            
            if (!string.IsNullOrEmpty(dataTable_.SearchValue))
            {
                Records = Records.Where(x => x.FileName.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.UserName.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.CAName.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.AllocatedByName.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.Remark.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.status.ToLower().Contains(dataTable_.SearchValue.ToLower()) || x.FileID.ToLower().Contains(dataTable_.SearchValue.ToLower()) || (x.Created_date.Contains(dataTable_.SearchValue))).ToList();
            }

            if (!string.IsNullOrEmpty(dataTable_.sortColumn) && !string.IsNullOrEmpty(dataTable_.sortColumnDirection))
            {

                IEnumerable<TaskView_Dto> TaskDetails = Records;

                switch (dataTable_.sortColumn)
                {

                    case "FileName":
                        TaskDetails = dataTable_.sortColumnDirection == "asc" ?
                                TaskDetails.OrderBy(u => u.FileName) : TaskDetails.OrderByDescending(u => u.FileName);
                        break;

                    case "UserName":
                        TaskDetails = dataTable_.sortColumnDirection == "asc" ?
                                TaskDetails.OrderBy(u => u.UserName) : TaskDetails.OrderByDescending(u => u.UserName);
                        break;

                    case "CAName":
                        TaskDetails = dataTable_.sortColumnDirection == "asc" ?
                                TaskDetails.OrderBy(u => u.CAName) : TaskDetails.OrderByDescending(u => u.CAName);
                        break;

                    case "AllocatedByName":
                        TaskDetails = dataTable_.sortColumnDirection == "asc" ?
                                TaskDetails.OrderBy(u => u.AllocatedByName) : TaskDetails.OrderByDescending(u => u.AllocatedByName);
                        break;
                    case "Remark":
                        TaskDetails = dataTable_.sortColumnDirection == "asc" ?
                                TaskDetails.OrderBy(u => u.Remark) : TaskDetails.OrderByDescending(u => u.Remark);
                        break;
                    case "status":
                        TaskDetails = dataTable_.sortColumnDirection == "asc" ?
                            TaskDetails.OrderBy(u => u.status) : TaskDetails.OrderByDescending(u => u.status);
                        break;

                    case "FileID":
                        TaskDetails = dataTable_.sortColumnDirection == "asc" ?
                                TaskDetails.OrderBy(u => u.FileID) : TaskDetails.OrderByDescending(u => u.FileID);
                        break;
                    case "Created_date":
                        TaskDetails = dataTable_.sortColumnDirection == "asc" ?
                                TaskDetails.OrderBy(u => u.Created_date) : TaskDetails.OrderByDescending(u => u.Created_date);
                        break;

                }



                Records = TaskDetails.ToList();

            }

            return Records;
        }
    }
}