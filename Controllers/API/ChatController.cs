using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Trippin_Website.Hubs;
using Trippin_Website.Models;

namespace Trippin_Website.Controllers.API
{
    [AllowAnonymous]
    public class ChatController : ApiController
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext _hubContext;

        public ChatController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
        }

        [HttpPost]
        [Route("MarkThisMessageAsRead/{messageId}")]
        public async Task<IHttpActionResult> MarkThisMessageAsRead(string messageId)
        {
            var message = await _context.ChatContents.SingleOrDefaultAsync(m => m.Id.ToString() == messageId);
            if (message != null)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();

                var receiverId = message.ReceiverId;
                var group = $"chat-{message.ChatId}";

                return Ok();
            }
            return NotFound();
        }

        [HttpPost]
        [Route("MarkMessagesAsRead/{chatId}/{howMany}")]
        public async Task<IHttpActionResult> MarkMessagesAsRead(List<string> Ids, string chatId, int howMany)
        {
            if (Ids == null)
                return BadRequest();

            var userId = User.Identity.GetUserId();

            var userMessages = _context.ChatContents.Where(c => c.SenderId != userId && c.ChatId == chatId).Take(howMany);

            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            foreach (var message in userMessages)
            {
                message.IsRead = true;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("ClearChat/{chatId}")]
        public async Task<IHttpActionResult> ClearConversation(string chatId)
        {
            var currentUserId = User.Identity.GetUserId();
            var isMemberInConversation = await _context.ChatMembers.AnyAsync(c => c.ChatId == chatId && c.MemberId == currentUserId);
            var thisChatContents = _context.ChatContents.Where(c => c.ChatId == chatId);

            if (chatId == null || !isMemberInConversation)
                return BadRequest("Nu ai voie sa faci asta!");

            _context.ChatContents.RemoveRange(thisChatContents);

            _context.SaveChanges();

            return Ok();
        }


        [HttpPost]
        [Route("MarkAllAsRead/{chatId}/{userId}")]
        public async Task<IHttpActionResult> MarkAllAsRead(string chatId, string userId)
        {
            var messages = _context.ChatContents
                .Where(m => m.ChatId == chatId && m.ReceiverId == userId && m.IsRead == false)
                .ToList();

            foreach (var message in messages)
            {
                message.IsRead = true;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }


    }
}