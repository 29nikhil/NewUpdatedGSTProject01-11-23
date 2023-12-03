using Data_Access_Layer.Db_Context;
using Data_Access_Layer.Models;
using Repository_Logic.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Logic.MessageChatRepository
{
    public class Message:IMessage
    {

         private readonly List<MessageChat> _messages = new List<MessageChat>();
        private readonly Application_Db_Context _Context;

        public Message( Application_Db_Context context)
        {
            _Context = context;
        }

        public IEnumerable<Application_User> ShowAllUsers(string UserId, string Search)
        {
            var Userlist = _Context.appUser
                .Where(x => x.Id != UserId)
                .Select(x => new Application_User
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    UserName = x.UserName  // Add this line to include UserName in the projection
                })
                .ToList();

            if (!string.IsNullOrEmpty(Search))
            {
                string searchValue = Search.ToLower();
                Userlist = Userlist.Where(x =>
                    x.FirstName.ToLower().Contains(searchValue) ||
                    x.LastName.ToLower().Contains(searchValue) ||
                    x.Email.ToLower().Contains(searchValue)
                ).ToList();
            }

            return Userlist; // Return the filtered or unfiltered user list
        }
        void AddMessage(Message_Dto message)
        {

            MessageChat messageChat = new MessageChat();
            messageChat.SenderId = message.SenderId;
            messageChat.ReceiverId = message.ReceiverId;
            messageChat.SentAt = DateTime.Now;
            messageChat.ReadAt = DateTime.Now;
            messageChat.Status = MessageStatus.Sent;
            messageChat.Text = message.Text;
            // Implementation to add a message to the database
            _Context.MessageChat.Add(messageChat);
            _Context.SaveChanges();
        }

        public IEnumerable<MessageChat> GetMessagesForUsers(string senderUserId, string receiverUserId)
        {
            // Implementation to retrieve messages from the database based on sender and receiver
            var messages = _Context.MessageChat
                .Where(m => (m.SenderId == senderUserId && m.ReceiverId == receiverUserId) ||
                            (m.SenderId == receiverUserId && m.ReceiverId == senderUserId))
                .OrderBy(m => m.SentAt)
                .ToList();

            return messages;
        }

        void IMessage.AddMessage(Message_Dto message)
        {
            MessageChat messageChat = new MessageChat();
            messageChat.SenderId = message.SenderId;
            messageChat.ReceiverId = message.ReceiverId;
            messageChat.SentAt = DateTime.Now;
            messageChat.ReadAt = DateTime.Now;
            messageChat.Status = MessageStatus.Sent;
            messageChat.Text = message.Text;
            // Implementation to add a message to the database
             _Context.MessageChat.Add(messageChat);
            _Context.SaveChanges();
        }
    }
}
