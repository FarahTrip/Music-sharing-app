using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Trippin_Website.Models;
using Trippin_Website.ViewModels;

namespace Trippin_Website.Controllers
{
    public class PieseController : Controller
    {
        private ApplicationDbContext _context;
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public PieseController()
        {
            _context = new ApplicationDbContext();

        }
        // GET: Piese
        public ActionResult Index()
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

        [Route("Piese/detalii/{id?}")]
        public ActionResult Detalii(int? id)
        {
            var piese = _context.Piese.Include(c => c.Style).Include(c => c.PiesaFileName).SingleOrDefault(c => c.Id == id);
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            var viewModel = new PieseStiluriViewModel
            {
                Piese = piese,
                Style = _context.StyleOf.ToList(),
                PieseFileNames = _context.PieseFileNames.ToList()

            };
            return View(viewModel);
        }

        public ActionResult AdaugaNou()
        {
            var Stiluri = _context.StyleOf.ToList();
            var viewModel = new PieseStiluriViewModel
            {
                Style = Stiluri
            };
            return View(viewModel);
        }
        public ActionResult Creeaza(Piese piese)
        {
            if (!ModelState.IsValid)
            {
                var PieseModel = new PieseStiluriViewModel()
                {
                    Piese = piese,
                    Style = _context.StyleOf.ToList(),
                };
                return View("AdaugaNou", PieseModel);
            }
            _context.Piese.Add(piese);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ModificaFileName(Piese piese)
        {
            var CurrentDateTime = DateTime.Now;
            var PiesaInDB = _context.Piese.Single(c => c.Id == piese.Id);

            try
            {
                PiesaInDB.PiesaFileNameId = piese.PiesaFileNameId;
                PiesaInDB.DateModified = CurrentDateTime;
                _context.SaveChanges();
                return RedirectToAction("Detalii", "Piese", new { piese.Id });
            }
            catch
            {
                return Content("Eroare in sloboz");
            }

        }
        public ActionResult AlegePiesa(int? id)
        {
            var piese = _context.Piese.SingleOrDefault(c => c.Id == id);
            if (id == null || id == 0)
            {
                return RedirectToAction("Detalii");
            }
            var viewModel = new PieseAndPieseFileNamesViewModel
            {
                Piese = piese,
                PieseFileNames = _context.PieseFileNames.ToList()
            };
            return View(viewModel);
        }

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
        //----------------------------------------------------------------------- come back here 
        public ActionResult ModificaPiesa(int id)
        {
            if (!ModelState.IsValid)
            {

                var ModelForValidation = new PieseStiluriViewModel()
                {

                };

            }
            var piesa = _context.Piese.SingleOrDefault(c => c.Id == id);
            var stiluri = _context.StyleOf.ToList();
            var Model = new PieseStiluriViewModel()
            {
                Piese = piesa,
                Style = stiluri
            };
            return View(Model);
        }
        public ActionResult ModificaSaved(PieseStiluriViewModel PiesaModel)
        {
            var CurrentDateTime = DateTime.Now;
            var PieseInDb = _context.Piese.Single(c => c.Id == PiesaModel.Piese.Id);
            PieseInDb.Name = PiesaModel.Piese.Name;
            PieseInDb.Key = PiesaModel.Piese.Key;
            PieseInDb.Style = PiesaModel.Piese.Style;
            PieseInDb.Description = PiesaModel.Piese.Description;
            PieseInDb.Bpm = PiesaModel.Piese.Bpm;
            PieseInDb.DateModified = CurrentDateTime;
            _context.SaveChanges();
            return RedirectToAction("Index", "Piese");

        }
    }
}