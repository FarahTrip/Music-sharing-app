using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Trippin_Website.Models;

namespace Trippin_Website.Controllers
{
    public class UploadController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        private readonly UserManager<ApplicationUser> _userManager;
        public UploadController()
        {
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            _context = new ApplicationDbContext();
        }

        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult UploadPiesa()
        {
            return View();
        }

        public ActionResult UploadProfilePicture(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/Content/Images/User Profiles Images"), _FileName);
                var userId = User.Identity.GetUserId();

                var user = _userManager.FindById(userId);

                string extension = Path.GetExtension(file.FileName).ToLower();
                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".bmp" || extension == ".webp")
                {
                    file.SaveAs(_path);

                    string _FileName2 = Path.GetFileName(file.FileName);

                    user.ProfilePicture = _FileName2;
                    _userManager.Update(user);

                    return RedirectToAction("Profile", "UsersManagement", new { Id = userId });
                }
                else
                {
                    ViewBag.Message = "Doar imaginile sunt acceptate ca format!";
                    return RedirectToAction("Profile", "UsersManagement", new { Id = userId });
                }
            }

            return View("Index");
        }
    }
}

