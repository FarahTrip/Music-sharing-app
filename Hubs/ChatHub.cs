using Microsoft.AspNet.SignalR;

namespace Trippin_Website.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }

    }
}