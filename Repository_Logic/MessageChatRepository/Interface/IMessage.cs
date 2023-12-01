using Data_Access_Layer.Models;
using Repository_Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.MessageChatRepository
{
    public interface IMessage
    {
        void AddMessage(Message_Dto message);
        public IEnumerable<MessageChat> GetMessagesForUsers(string senderUserId, string receiverUserId);
        public IEnumerable<Application_User> ShowAllUsers(string UserId,  string Search);

    }
}
