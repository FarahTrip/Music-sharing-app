using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Trippin_Website.Models;
using Trippin_Website.ViewModels;


namespace Trippin_Website.Controllers
{

    public class UsersManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UsersManagementController()
        {
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            _context = new ApplicationDbContext();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var users = _userManager.Users.ToList();
            var roles = _roleManager.Roles.ToList();

            ViewBag.Roles = roles;

            var viewModel = new UserListViewModel
            {
                Users = users
            };

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Profile(String Id)
        {

            var user = _userManager.Users.SingleOrDefault(c => c.Id == Id);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var Model = new User_Content_ViewModel
            {
                User = user,
                Piese = _context.Piese.ToList()
            };

            return View(Model);

        }

        public ActionResult DeleteProfilePicture()
        {
            var userId = User.Identity.GetUserId();

            var user = _userManager.FindById(userId);

            var Model = new User_Content_ViewModel
            {
                User = user,
                Piese = _context.Piese.ToList()
            };

            string path = Path.Combine(Server.MapPath("~/Content/Images/User Profiles Images/"), user.ProfilePicture);
            if ((System.IO.File.Exists(path)))
            {
                System.IO.File.Delete(path);
            }
            user.ProfilePicture = null;
            _userManager.Update(user);

            return View("Profile", Model);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult ChangeRole(string userId, string roleId)
        {
            var user = _userManager.FindById(userId);
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (string.IsNullOrEmpty(roleId))
            {
                var currentRoleId = user.Roles.FirstOrDefault()?.RoleId;
                if (currentRoleId != null)
                {
                    _userManager.RemoveFromRole(userId, _roleManager.FindById(currentRoleId).Name);
                }
            }
            else
            {
                var currentRoleId = user.Roles.FirstOrDefault()?.RoleId;
                if (currentRoleId != roleId)
                {
                    if (currentRoleId != null)
                    {
                        _userManager.RemoveFromRole(userId, _roleManager.FindById(currentRoleId).Name);
                    }

                    _userManager.AddToRole(userId, _roleManager.FindById(roleId).Name);
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


    }
}