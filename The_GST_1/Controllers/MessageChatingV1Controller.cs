using Data_Access_Layer.Db_Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Repository_Logic.Dto;
using Repository_Logic.MessageChatRepository;
using Repository_Logic.UserOtherDatails.Interface;
using System.Security.Claims;
using System.Web.Providers.Entities;
using The_GST_1.ChatHubd;
namespace The_GST_1.Controllers
{
    public class MessageChatingV1Controller : Controller
    {


        private readonly IHubContext<ChatHub> _hubContext;

        private readonly IMessage _message;
        private readonly IExtraDetails _User;
        private readonly Application_Db_Context _context;
        public MessageChatingV1Controller(IMessage message, IExtraDetails User, Application_Db_Context context, IHubContext<ChatHub> hubContext)
        {
            _message = message;
            //_userManager = userManager;
            _User = User;
            _hubContext = hubContext;
            //_context = context;
        }

        public async Task< IActionResult> MessageChatView()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var Uselist = _message.ShowAllUsers(userId,null); 
          
            ViewBag.Uselist = Uselist;
           
                
            var UserData = await _User.ShowInfirmationUsers(userId);
            ViewBag.UserName = UserData.FirstName + " " + UserData.LastName;
            ViewBag.Email = UserData.Email;
            return View();

        }
        [HttpPost]
        public IActionResult Search(string searchTerm)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var Uselist = _message.ShowAllUsers(userId , searchTerm); ;


            // Perform search logic here
            // You can query a database or any other data source

            // For demonstration purposes, just returning a sample result
            var result = $"Search result for: {searchTerm}";

            return Json(Uselist);
        }

        public async Task<IActionResult> GetUser(string SelectUserId)
        {

            //var UserData = await _User.ShowInfirmationUsers(SelectUserId);
          
            return new JsonResult(SelectUserId);

        }

        public async Task<IActionResult> ShowChatUser(string SelectUserId)
        {

            var UserData = await _User.ShowInfirmationUsers(SelectUserId);
            ViewBag.UserName = UserData.FirstName + " " + UserData.LastName;
            ViewBag.Email = UserData.Email;
            ViewBag.RecieverId = UserData.Id;
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.CurrentLoginUseid = userId;
            var messages = _message.GetMessagesForUsers(userId, SelectUserId);
            ViewBag.SenderUserId = userId;
            ViewBag.ReceiverUserId = SelectUserId;
            ViewBag.Message = messages;
            ViewBag.RefreshInterval = 5000;
            return View(messages);
           

        }


        
        public async Task<JsonResult> SendMessage( string receiverId,string messageContent)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (messageContent != null)
            {
                Message_Dto message = new Message_Dto();
                message.ReceiverId = receiverId;
                message.SenderId = userId;
                message.Text = messageContent;
                TempData["Refresh"] = "ee";
                _message.AddMessage(message);
                var messageschatlist = _message.GetMessagesForUsers(userId, receiverId);
                var UserData = await _User.ShowInfirmationUsers(receiverId);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", userId, messageContent);
                return Json(UserData);

            }
            return Json(true);
        }










        public async Task<JsonResult> ShowInformationUsers(string UserId)
        {
            var UserData = await _User.ShowInfirmationUsers(UserId);


            return new JsonResult(UserData);
        }





        public IActionResult C (string senderUserId, string receiverUserId)
        {
            var messages = _message.GetMessagesForUsers(senderUserId, receiverUserId);
            ViewBag.SenderUserId = senderUserId;
            ViewBag.ReceiverUserId = receiverUserId;
            return View(messages);
        }


    }
}
