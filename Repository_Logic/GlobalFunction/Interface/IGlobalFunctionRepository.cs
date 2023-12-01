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

        public int countUser();
        public int AllowcatedTaskDone();
        public int AllowcatedTaskTotal();
        public int FellowshipAllowcatedTaskTotal(string id);

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


    }
}
