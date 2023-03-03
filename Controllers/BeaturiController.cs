using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Trippin_Website.Models;
using Trippin_Website.ViewModels;

namespace Trippin_Website.Controllers
{
    public class BeaturiController : Controller
    {
        private ApplicationDbContext _context;
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public BeaturiController()
        {
            _context = new ApplicationDbContext();

        }
        // GET: Piese
        public ActionResult Index()
        {
            var viewModel = new BeaturiViewModel()
            {
                Beaturi = _context.Beaturi.ToList(),
                Stiluri = _context.StyleOf.ToList()

            };
            return View(viewModel);
        }

        [Route("Beaturi/detalii/{id?}")]
        public ActionResult Detalii(int? id)
        {
            var beaturi = _context.Beaturi.Include(c => c.Style).SingleOrDefault(c => c.Id == id);
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            var viewModel = new BeatViewModel
            {
                Beat = beaturi,
                Styles = _context.StyleOf.ToList()
            };
            return View(viewModel);
        }

        public ActionResult Edit()
        {
            var beaturi = _context.Beaturi.ToList();
            return View(beaturi);
        }


        public ActionResult AdaugaNou()
        {

            var StiluriList = _context.StyleOf.ToList();
            var viewModel = new BeatViewModel
            {
                Styles = StiluriList,
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creeaza(Beat Beat)
        {
            if (!ModelState.IsValid)
            {
                var Stiluri = _context.StyleOf.ToList();
                var viewModelForState = new BeatViewModel
                {
                    Styles = Stiluri,
                };
                return View("AdaugaNou", viewModelForState);
            }
            _context.Beaturi.Add(Beat);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult Modifica(Beat Beat)
        {
            var CurrentDateTime = DateTime.Now;
            var BeatInDb = _context.Beaturi.Single(c => c.Id == Beat.Id);
            BeatInDb.Name = Beat.Name;
            BeatInDb.Key = Beat.Key;
            BeatInDb.Style = Beat.Style;
            BeatInDb.Description = Beat.Description;
            BeatInDb.Bpm = Beat.Bpm;
            BeatInDb.Modified = CurrentDateTime;
            _context.SaveChanges();
            return RedirectToAction("Index", "Beaturi");
        }

        public ActionResult EditBeat(int Id)
        {
            var beat = _context.Beaturi.SingleOrDefault(c => c.Id == Id);
            if (beat == null)
            {
                return HttpNotFound();
            }
            else
            {
                var viewModel = new BeatViewModel
                {
                    Beat = beat,
                    Styles = _context.StyleOf.ToList()
                };
                return View(viewModel);

            }
        }
    }
}
