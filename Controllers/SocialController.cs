using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Trippin_Website.Models;

namespace Trippin_Website.Controllers
{
    [Authorize]
    public class SocialController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public SocialController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }
        // GET: Social
        public async Task<ActionResult> Like(Guid Id)
        {
            var user = _userManager.FindById(User.Identity.GetUserId());
            var Piesa = await _context.Piese.FindAsync(Id);
            if (Id == null)
            {
                return HttpNotFound();
            }

            var LikedBefore = await _context.Likes.SingleOrDefaultAsync(c => c.UserId == user.Id);
            if (LikedBefore == null)
            {
                var like = new Likes
                {
                    UserId = user.Id,
                    PiesaId = (Guid)Piesa.Id
                };

                _context.Likes.Add(like);
                Piesa.Likes++;
                await _context.SaveChangesAsync();

                return View("Detalii", "Piese", new { id = Id });

            }
            if (LikedBefore != null)
            {
                _context.Likes.Remove(LikedBefore);
                Piesa.Likes--;

                await _context.SaveChangesAsync();

                return View("Detalii", "Piese", new { id = Id });
            }

            return View("Detalii", "Piese", new { id = Id });

        }

        public ActionResult ChatWithUser(string otherUserId)
        {
            var currentUserId = User.Identity.GetUserId();
            var otheruser = _userManager.FindById(otherUserId);
            var currentUserChats = _context.ChatMembers
                .Where(m => m.MemberId == currentUserId)
                .Select(m => m.ChatId)
                .ToList();

            var otherUserChats = _context.ChatMembers
                .Where(m => m.MemberId == otherUserId)
                .Select(m => m.ChatId)
                .ToList();

            var commonChats = currentUserChats.Intersect(otherUserChats).ToList();

            // If there is an existing chat, use that. Otherwise, create a new one.
            var chatId = commonChats.FirstOrDefault();
            if (chatId == null)
            {
                var chat = new Chat { Id = Guid.NewGuid() };
                chatId = chat.Id.ToString();

                _context.Chats.Add(chat);
                _context.ChatMembers.Add(new ChatMembers { Id = Guid.NewGuid(), MemberId = currentUserId, ChatId = chatId });
                _context.ChatMembers.Add(new ChatMembers { Id = Guid.NewGuid(), MemberId = otherUserId, ChatId = chatId });

                _context.SaveChanges();
            }

            return RedirectToAction("Chat", new { id = chatId, otherUserName = otheruser.UserName, otherUserId = otherUserId });
        }
        public ActionResult Chat(Guid id, string otherUserName, string otherUserId)
        {
            var currentId = User.Identity.GetUserId();
            var currentUser = _userManager.FindById(currentId);

            var chatMembers = _context.ChatMembers
     .Where(m => m.ChatId == id.ToString())
     .ToList();

            if (!chatMembers.Any(m => m.MemberId == currentId))
            {
                return RedirectToAction("Index", "Home");
            }

            var chatHistory = _context.ChatContents
                .Where(m => m.ChatId == id.ToString())
                .OrderBy(m => m.SentAt)
                .ToList();

            StringBuilder chatHistoryHtml = new StringBuilder();

            foreach (var message in chatHistory/*.Take(30)*/)
            {
                var user = _userManager.FindById(message.SenderId);
                string profilePicPath = $"/Content/Images/User Profiles Images/{user.ProfilePicture}";
                string messageClass = message.SenderId == currentId ? "chat-message self" : "chat-message";

                string eyeIconClass = message.SenderId == currentId ? "fa-regular fa-eye" : "";
                if (message.IsRead)
                {
                    eyeIconClass += " read";
                }
                string eyeIconHtml = $"<i class=\"{eyeIconClass}\"></i>";
                string messageHtml = $@"<div class='{messageClass}'><img src='{profilePicPath}' alt='Profile Picture'><div class='message-text'><div class='message-author'>{message.UserName}</div><div class='message-body'>{message.Message}</div></div>{eyeIconHtml}</div>";
                chatHistoryHtml.Append(messageHtml);
            }

            ViewBag.ChatHistoryHtml = chatHistoryHtml.ToString();

            ViewBag.OtherUserId = otherUserId;
            ViewBag.ChatId = id;
            ViewBag.profilePicture = currentUser.ProfilePicture;
            ViewBag.otherUserName = otherUserName;
            ViewBag.currentUserName = currentUser.UserName;
            ViewBag.currentUserID = currentId;

            return View();
        }
    }
}