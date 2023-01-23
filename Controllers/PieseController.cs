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
            return View(piese);
        }

        [Route("Piese/detalii/{id?}")]
        public ActionResult Detalii(int? id)
        {
            var piese = _context.Piese.Include(c => c.Style).SingleOrDefault(c => c.Id == id);
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            var viewModel = new PieseStiluriViewModel
            {
                Piese = piese,
                Style = _context.StyleOf.ToList()
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
            _context.Piese.Add(piese);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}