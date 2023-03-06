using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trippin_Website.Models;
using Trippin_Website.ViewModels;

namespace Trippin_Website.Controllers
{
    public class PieseController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public PieseController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }
        // GET: Piese

        [Authorize(Roles = "Admin, Producer")]
        public ActionResult Index()
        {
            var piese = _context.Piese.ToList();
            var Stiluri = _context.StyleOf.ToList();
            var PieseFileNames = _context.PieseFileNames.ToList();
            var users = _userManager.Users.ToList();
            var pieseAllViewModel = new PieseAllViewModel()
            {
                Piese = piese,
                PieseFileNames = PieseFileNames,
                Stiluri = Stiluri,
                Users = users
            };

            var pieseIndexViewModel = new PieseIndexViewModel()
            {
                PieseAllViewModel = pieseAllViewModel,
                UserManager = _userManager
            };

            return View(pieseIndexViewModel);
        }

        [AllowAnonymous]
        [Route("Piese/detalii/{id?}")]
        public ActionResult Detalii(Guid? id)
        {
            var piese = _context.Piese.Include(c => c.Style).SingleOrDefault(c => c.Id == id);
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var viewModel = new PieseViewModel
            {
                Piese = piese,
                Style = _context.StyleOf.ToList()
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Admin, Producer")]
        public ActionResult AdaugaNou()
        {
            var Stiluri = _context.StyleOf.ToList();
            var viewModel = new PieseViewModel
            {
                Style = Stiluri
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Producer")]
        public ActionResult Creeaza(Piese piese, HttpPostedFileBase file)
        {
            var PieseModel = new PieseViewModel()
            {
                Piese = piese,
                Style = _context.StyleOf.ToList(),
            };

            if (!ModelState.IsValid)
            {

                return View("AdaugaNou", PieseModel);
            }

            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Piese-Uploaded"), _FileName);

                    string extension = Path.GetExtension(file.FileName).ToLower();
                    if (extension == ".wav" || extension == ".mp3")
                    {
                        file.SaveAs(_path);
                        piese.FileName = _FileName;
                    }
                    else
                    {
                        ViewBag.Message = "Doar .wav si .mp3 sunt acceptate ca si extensii!.";
                        return View("AdaugaNou", PieseModel);
                    }
                }
                else
                {
                    ViewBag.Message = "Te rog selecteaza un fisier!";
                    return View("AdaugaNou", PieseModel);
                }

                piese.Id = Guid.NewGuid();
                piese.UserId = User.Identity.GetUserId();
                _context.Piese.Add(piese);
                _context.SaveChanges();

                return RedirectToAction("Index", "Piese");
            }
            catch
            {
                ViewBag.Message = "Te rog selecteaza un fisier!!";
                return View("AdaugaNou", PieseModel);
            }
        }

        [Authorize(Roles = "Admin, Producer")]
        public ActionResult Modifica()
        {
            var piese = _context.Piese.ToList();
            var Stiluri = _context.StyleOf.ToList();
            var PieseFileNames = _context.PieseFileNames.ToList();
            var pieseModel = new PieseAllViewModel()
            {
                Piese = piese,
                PieseFileNames = PieseFileNames,
                Stiluri = Stiluri,
            };

            return View(pieseModel);
        }

        [Authorize(Roles = "Admin, Producer")]
        public ActionResult ModificaPiesa(Guid id)
        {
            if (!ModelState.IsValid)
            {
                var piesaForValidation = _context.Piese.SingleOrDefault(c => c.Id == id);
                var ModelForValidation = new PieseViewModel
                {
                    Piese = piesaForValidation,
                    Style = _context.StyleOf.ToList()
                };
                return View(ModelForValidation);
            }
            var piesa = _context.Piese.SingleOrDefault(c => c.Id == id);
            var stiluri = _context.StyleOf.ToList();
            var Model = new PieseViewModel()
            {
                Piese = piesa,
                Style = stiluri
            };
            return View(Model);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Producer")]
        public ActionResult ModificaSaved(PieseViewModel PiesaModel)
        {
            var CurrentDateTime = DateTime.Now;
            var PieseInDb = _context.Piese.Single(c => c.Id == PiesaModel.Piese.Id);
            PieseInDb.Name = PiesaModel.Piese.Name;
            PieseInDb.Key = PiesaModel.Piese.Key;
            PieseInDb.StyleId = PiesaModel.Piese.StyleId;
            PieseInDb.Description = PiesaModel.Piese.Description;
            PieseInDb.Bpm = PiesaModel.Piese.Bpm;
            PieseInDb.IsBanger = PiesaModel.Piese.IsBanger;
            PieseInDb.DateModified = CurrentDateTime;
            _context.SaveChanges();
            return RedirectToAction("Index", "Piese");

        }
    }
}