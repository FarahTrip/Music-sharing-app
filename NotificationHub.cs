using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace Trippin_Website
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("ReceiveMessage");
        }
    }
}