using Amazon;
using Amazon.S3;
using Amazon.S3.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Trippin_Website.Logic_classes;
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
        [Route("profile/{id:guid}")]
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
                Piese = _context.Piese.OrderByDescending(c => c.DateCreated).ToList(),
                Beaturi = _context.Beaturi.OrderByDescending(c => c.Created).ToList(),
            };

            var ProfileUser = _userManager.FindById(user.Id);
            if (ProfileUser.Roles.Any(c => c.RoleId == "47db5674-87ba-471d-ad7c-7c4aae7958d8"))
                ViewBag.UserProfileRole = "Admin";

            if (ProfileUser.Roles.Any(c => c.RoleId == "60996260-1397-4b73-b5d4-a4b484fb554d"))
                ViewBag.UserProfileRole = "Artist";

            if (ProfileUser.Roles.Any(c => c.RoleId == "90ae9446-6d60-44e7-99b7-97a7031b1d2c"))
                ViewBag.UserProfileRole = "Producer";


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
        public async Task<ActionResult> ChangeRole(string userId, string roleId)
        {
            var user = _userManager.FindById(userId);
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                if (string.IsNullOrEmpty(roleId))
                {
                    var currentRoleId = user.Roles.FirstOrDefault()?.RoleId;
                    if (currentRoleId != null)
                    {
                        await _userManager.RemoveFromRoleAsync(userId, _roleManager.FindById(currentRoleId).Name);
                    }
                }
                else
                {
                    var currentRoleId = user.Roles.FirstOrDefault()?.RoleId;
                    if (currentRoleId != roleId)
                    {
                        if (currentRoleId != null)
                        {
                            await _userManager.RemoveFromRoleAsync(userId, _roleManager.FindById(currentRoleId).Name);
                        }

                        _userManager.AddToRole(userId, _roleManager.FindById(roleId).Name);
                    }
                }
            }

            catch (Exception ex)
            {
                ViewBag.Eroare = $"A aparut o eroare neasteptata. Mesaj : {ex.Message} StackTrace : {ex.StackTrace}";
            }


            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public async Task<ActionResult> DeleteUser(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());

            var usersList = _userManager.Users.ToList();
            var roles = _roleManager.Roles.ToList();
            var currentUserId = User.Identity.GetUserId();

            ViewBag.Roles = roles;
            var users = new UserListViewModel
            {
                Users = usersList
            };


            if (Id == null || user == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);



            if (User.IsInRole("Admin") || user.Id == currentUserId)
            {
                try
                {
                    var logins = await _userManager.GetLoginsAsync(user.Id);
                    var role = await _userManager.GetRolesAsync(user.Id);

                    var pieseUser = _context.Piese.Where(c => c.UserId == user.Id);
                    var beaturiUser = _context.Beaturi.Where(c => c.UserId == user.Id);
                    var likesUser = _context.Likes.Where(c => c.UserId == user.Id);

                    var amazonHelper = new AmazonHelper();
                    var client = new AmazonS3Client(amazonHelper.AccessId, amazonHelper.SecretKey, RegionEndpoint.EUNorth1);
                    var folderKey = $"Users-Files/{Id}";
                    var amazonFolder = new S3DirectoryInfo(client, amazonHelper.BucketName, folderKey);
                    if (amazonFolder.Exists)
                        amazonFolder.Delete(true);

                    foreach (var login in logins.ToList())
                    {
                        await _userManager.RemoveLoginAsync(user.Id, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                    }

                    if (role != null)
                        await _userManager.RemoveFromRoleAsync(user.Id, user.Roles.FirstOrDefault().ToString());

                    if (pieseUser != null)
                        _context.Piese.RemoveRange(pieseUser);

                    if (beaturiUser != null)
                        _context.Beaturi.RemoveRange(beaturiUser);

                    if (likesUser != null)
                        _context.Likes.RemoveRange(likesUser);


                    _context.SaveChanges();

                    await _userManager.DeleteAsync(user);

                    ViewBag.Success = $"Succes!";

                    users.Users = _userManager.Users.ToList();

                    return View("Index", users);

                }
                catch (AmazonS3Exception ex)
                {
                    ViewBag.Eroare = $"A aparut o eroare neasteptata. Mesaj : {ex.Message} StackTrace : {ex.StackTrace}";
                    return View("Index", users);
                }
                catch (Exception ex)
                {
                    ViewBag.Eroare = $"A aparut o eroare neasteptata. Mesaj : {ex.Message} StackTrace : {ex.StackTrace}";
                    return View("Index", users);
                }
            }
            return View("Index", users);
        }

        public ActionResult Versuri(Guid? Id)
        {
            var user = _userManager.Users.SingleOrDefault(c => c.Id == Id.ToString());
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var versuri = new VersuriViewModel()
            {
                Versuri = _context.Versuri.ToList(),
                UserId = Id.ToString(),
            };


            return View(versuri);
        }


        [HttpPost]
        public async Task<ActionResult> AdaugaVers(VersuriViewModel model)
        {
            var userId = User.Identity.GetUserId();

            if (!ModelState.IsValid)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            model.Vers.Id = Guid.NewGuid();
            model.Vers.UserId = userId;
            model.Vers.Created = DateTime.Now;

            _context.Versuri.Add(model.Vers);
            await _context.SaveChangesAsync();

            return RedirectToAction("Versuri", "UsersManagement", new { id = userId });
        }
    }
}