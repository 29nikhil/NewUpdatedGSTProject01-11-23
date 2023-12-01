using Data_Access_Layer.Models;
using Repository_Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.FellowshipRepository.Interface
{
    public interface IFellowshipRepository
    {
        public IEnumerable<Application_User> GetAllFellowshipRecord();
        Application_User GetFellowShipṚeccord(string id);
        void DeleteFellowship(string id);
        void UpdateFellowship(Application_User user);
        public IEnumerable<Application_User> GetAllFellowshipRecord(DataTable_Dto dataTable_Dto);

        public  Task<string> GetFellowshisession(string id);
        public string GetIDByUserName(string UserName);
        public string GetCA(string id);

    }
}
