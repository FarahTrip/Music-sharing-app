using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
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
    }
}