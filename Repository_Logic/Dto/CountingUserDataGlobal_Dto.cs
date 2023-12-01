using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.Dto
{
    public class CountingUserDataGlobal_Dto
    {
        public int TotalUser { get; set; }
        public int TotalFellowship { get; set; }
        public int AllUser { get; set; }
        public int AllFellowship { get; set; }
        public int ReturnFileCount { get; set; }
        public int TaskCount { get; set; }
        public int PendingChanges { get; set; }
        public int ConfirmAccounts { get; set; }
       
        public int NotConfirmAccountsUser { get; set; }



    }
}
