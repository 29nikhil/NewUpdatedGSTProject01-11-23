using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Repository_Logic.Dto;
using Repository_Logic.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.UserOtherDatails.Interface
{
    public interface IExtraDetails
    {
        Task Add(UserOtherDetails_Dto otherDetails); //
       
      
      //  IEnumerable<Application_User> GetAllFellowship();
        IEnumerable<UserModelView> GetAllUser();//
        IEnumerable<JoinUserTable_Dto> GetAllUserList();//

        JoinUserTable_Dto GetUser(string id);
      //  Application_User GetFellowShip(string id);

        void DeleteUser(string id);
     //   void DeleteFellowship(string id);
        void UpdateUser(JoinUserTable_Dto user);
        //Update Email Id then Email Status Change
        void UpdateEmailConfirmation(string UserId);

        //  void UpdateFellowship(Application_User user);
        Task<int> GetUserEmailConfirmCountAll();
        public Task<int> GetUserNotConfirmedcountAll();


        // this function 
        public IEnumerable<Application_User> ShowAllUsers();

        public Task<Application_User> ShowInfirmationUsers(string Userid);
        public string AvaibleEmail(string email);
        public string AvaibleGstNo(string gstNo);


    }
}
