using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Web.Http;
using Trippin_Website.Models;


namespace Trippin_Website.Controllers.API
{
    [Authorize]
    public class SocialApiController : ApiController
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public SocialApiController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }


        [HttpGet]
        public IHttpActionResult GetLikes(Guid Id)
        {
            var user = _userManager.FindById(User.Identity.GetUserId());
            var Piesa = _context.Piese.SingleOrDefault(c => c.Id == Id);
            var likes = _context.Likes.Where(c => c.PiesaId == Id).ToList();

            if (Piesa == null)
                return NotFound();

            var like = _context.Likes.SingleOrDefault(l => l.UserId == user.Id && l.PiesaId == Piesa.Id);

            if (like == null)
            {
                return Json(new { likes = likes.Count, alreadyLiked = false });
            }
            else
            {
                return Json(new { likes = likes.Count, alreadyLiked = true });
            }
        }


        [HttpPost]
        public IHttpActionResult Like(Guid Id)
        {
            var user = _userManager.FindById(User.Identity.GetUserId());
            var Piesa = _context.Piese.Find(Id);

            if (Piesa == null)
            {
                return NotFound();
            }

            var like = _context.Likes.SingleOrDefault(l => l.UserId == user.Id && l.PiesaId == Piesa.Id);

            if (like == null)
            {
                like = new Likes
                {
                    UserId = user.Id,
                    PiesaId = (Guid)Piesa.Id
                };

                _context.Likes.Add(like);
                Piesa.Likes++;
                _context.SaveChanges();

                return Json(new { success = true, action = "like", count = Piesa.Likes });
            }
            else
            {
                _context.Likes.Remove(like);
                Piesa.Likes--;
                _context.SaveChanges();

                return Json(new { success = true, action = "unlike", count = Piesa.Likes });
            }
        }

        [HttpDelete]
        public IHttpActionResult Unlike(Guid Id)
        {
            var user = _userManager.FindById(User.Identity.GetUserId());
            var Piesa = _context.Piese.Find(Id);

            if (Piesa == null)
            {
                return NotFound();
            }

            var like = _context.Likes.SingleOrDefault(l => l.UserId == user.Id && l.PiesaId == Piesa.Id);

            if (like == null)
            {
                return Json(new { success = true, action = "unlike", count = Piesa.Likes });
            }
            else
            {
                _context.Likes.Remove(like);
                Piesa.Likes--;
                _context.SaveChanges();

                return Json(new { success = true, action = "unlike", count = Piesa.Likes });
            }
        }
    }
}
