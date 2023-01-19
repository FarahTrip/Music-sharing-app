using System.Linq;
using System.Web.Mvc;
using Trippin_Website.Models;

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
            var piese = _context.Piese.SingleOrDefault(c => c.Id == id);

            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(piese);
            }

        }

    }
}