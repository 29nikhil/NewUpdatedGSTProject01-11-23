using Azure.Messaging;
using Data_Access_Layer.Db_Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Plugins;
using Repository_Logic.Dto;
using Repository_Logic.MessageChatRepository;
using Repository_Logic.UserOtherDatails.Interface;
using System.Security.Claims;
using System.Web.Providers.Entities;

namespace The_GST_1.Controllers
{
    public class MessageChatingController : Controller
    {
        private readonly IMessage _message;
        private readonly IExtraDetails _User;
        private readonly Application_Db_Context _context;
        public MessageChatingController(IMessage message,  IExtraDetails User, Application_Db_Context context)
        {
            _message = message;
            //_userManager = userManager;
            _User = User;
            //_context = context;
        }

        public IActionResult MessageChatView()
        {
            var Uselist=_User.ShowAllUsers();
            ViewBag.Uselist = Uselist;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            return View();
           
        }

        public async Task<JsonResult> ShowInformationUsers(string UserId)
        {
            var UserData = await _User.ShowInfirmationUsers(UserId);
          

            return new JsonResult(UserData);
        }



        // Server-side code
        public async Task<JsonResult> SendMessage(string messageContent, string receiverId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Message_Dto message = new Message_Dto();
            message.ReceiverId = receiverId;
            message.SenderId = userId;
            message.Text = messageContent;

            _message.AddMessage(message);
            var messageschatlist = _message.GetMessagesForUsers(userId, receiverId);
            var UserData = await _User.ShowInfirmationUsers(receiverId);


            return new JsonResult(UserData);
        }
        public IActionResult Index(string senderUserId, string receiverUserId)
        {
            var messages = _message.GetMessagesForUsers(senderUserId, receiverUserId);
            ViewBag.SenderUserId = senderUserId;
            ViewBag.ReceiverUserId = receiverUserId;
            return View(messages);
        }






        public  async Task< IActionResult> MessageChatView1(string Reciverid,string Text)
        {
            var Uselist = _User.ShowAllUsers();
            ViewBag.Uselist = Uselist;

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var messages = _message.GetMessagesForUsers(userId, Reciverid);
            ViewBag.Message = messages;
            var UserDataw =  _User.ShowInfirmationUsers(Reciverid);
            ViewBag.ReciverId = Reciverid;
           
            var userDataInfo =await _User.ShowInfirmationUsers(Reciverid);
            if(Reciverid!=null&&Text==null)
            {
                var userData = new
                {
                    Id = userDataInfo.Id,
                    UserNameFull = $"{userDataInfo.FirstName} {userDataInfo.LastName}",
                    Username = userDataInfo.UserName
                };

                ViewBag.Reciverid = userDataInfo.Id;
                ViewBag.UserFullName = userDataInfo.FirstName + " " + userDataInfo.LastName;
                ViewBag.Email = userDataInfo.Email;
                return Json(userData);

            }
            else if (Reciverid!=null&& Text!=null)
            {
                Message_Dto message = new Message_Dto();
                message.ReceiverId = Reciverid;
                message.SenderId = userId;
                message.Text = Text;

                _message.AddMessage(message);
                var userData = new
                {
                    Id = userDataInfo.Id,
                    UserNameFull = $"{userDataInfo.FirstName} {userDataInfo.LastName}",
                    Username = userDataInfo.UserName
                };

                return Json(userData);

            }
           

            return View();
        }

    }
}
