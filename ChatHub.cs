namespace Trippin_Website.Hubs
{
    using Microsoft.AspNet.SignalR;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Trippin_Website.Models;

    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        public ChatHub()
        {
            _context = new ApplicationDbContext();
        }
        public async Task Send(string name, string message, string profilePicture, string groupId, string senderId, string receiverId)
        {
            try
            {
                var chatMessage = new ChatContents
                {
                    Id = Guid.NewGuid(),
                    SenderId = senderId,
                    ChatId = groupId,
                    Message = message,
                    SentAt = DateTime.UtcNow,
                    UserName = name,
                    ReceiverId = receiverId
                };

                _context.ChatContents.Add(chatMessage);
                await _context.SaveChangesAsync();

                Clients.Group(groupId).addNewMessageToPage(name, message, profilePicture, chatMessage.Id);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task MarkMessagesAsReadForUser(string chatId, string currentUserId)
        {
            var messages = _context.ChatContents
                                   .Where(m => m.ChatId == chatId && m.SenderId != currentUserId && m.IsRead == false)
                                   .ToList();

            foreach (var message in messages)
            {
                message.IsRead = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task MarkMessageAsRead(string messageId, string senderId)
        {
            var message = _context.ChatContents.Find(messageId);
            if (message != null && message.SenderId == senderId)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        public Task JoinGroup(string groupId)
        {
            return Groups.Add(Context.ConnectionId, groupId);
        }

        public Task LeaveGroup(string groupId)
        {
            return Groups.Remove(Context.ConnectionId, groupId);
        }


    }
}