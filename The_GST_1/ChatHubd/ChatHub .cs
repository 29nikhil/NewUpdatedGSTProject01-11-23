namespace The_GST_1.ChatHubd
{// ChatHub.cs
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class ChatHub : Hub
    {
        public async Task SendMessage(string userId, string message)
        {
            // Send the message to all clients
            await Clients.All.SendAsync("ReceiveMessage", userId,message);

        }
    }

}
