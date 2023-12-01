using Data_Access_Layer.Models;
using Repository_Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.TaskAllocation.Interface
{
    public interface ITaskAllocation
    {
        public void Insert(AllocatedTask_Dto allocatedTask_Dto);
        public void InsertTask(TaskAllowcated_Dto allocatedTask_Dto);//New Update Insert Data Task Table
        public  Task<List<TaskView_Dto>> TaskListAsync(string SessionID); //new Update 0.1
        public Task<List<TaskView_Dto>> ViewTaskListDataTable(DataTable_Dto dataTable_, string LoginSessionID); //New Update 0.1

        public List<TaskList_Dto> GetDataBySessionID(string SessionID);
        public List<TaskList_Dto> GetDataBySessionIDForFellowship(string loginSessionID);

        public void ChangesDoneTask(string Id);//New Update 0.1

        public void ChangesDone(string FileID);

        public Task<List<TaskList_Dto>> ViewTaskDataTable(DataTable_Dto dataTable_, string LoginSessionID);

    }
}
