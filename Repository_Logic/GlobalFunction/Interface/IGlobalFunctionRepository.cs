using Microsoft.Identity.Client;
using Repository_Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.GlobalFunction.Interface
{
    public interface IGlobalFunctionRepository
    {
        public int CountReturnedFilesForUser(string LoginSessionID);
        public int countReturnedFilesAndGSTBillsSubmittedForUser(string LoginSessionID);
        public int countTotalFilesForUser(string LoginSessionID);
        public Task<int> countPendingTaskForFellowship(string LoginSessionID);
        public Task<int> countTotalFiles(string LoginSessionID);
        public Task<int> ReturnFilesForFellowship(string LoginSessionID);
        public Task<int> countReturnedFilesAndGSTBillsSubmittedForFellowship(string LoginSessionID);

        public Task<int> countFilesReturnedAndGSTBillsSubmittedForCA();

        public Task<int> countFilesReturnedAndGSTBillsNotSubmittedForCA();
      
        public int countUser();
        public int AllowcatedTaskDone();
        public int AllowcatedTaskTotal();
        public int FellowshipTotalFileUploded(string id);

        public int FellowshipAllowcatedTask(string id);
        public int FellowshipReturnFileCount(string id);
        public int UserSideReturnFileCount(string id);
        public int UserSideUploadFiles(string id);
        public int UserSideDoneChangesFile(string id);
        public int UserSidePendingChagesFile(string id);

        public int ReturnFileCountAll();
        public Task<int> TotalUser();
        public Task<int> TotalFellowship();
        public int IndividualUserType();
        public int OrganizationUserType();
        public Task<JoinUserTable_Dto> GetUserDataNavBar(string Id);
        public int YearlyGst();
        public int MonthlyGst();


    }
}
