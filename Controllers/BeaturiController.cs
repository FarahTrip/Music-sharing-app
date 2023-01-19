using System.Linq;
using System.Web.Mvc;
using Trippin_Website.Models;

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
            var beaturi = _context.Beaturi.ToList();
            return View(beaturi);
        }

        [Route("Beaturi/detalii/{id?}")]
        public ActionResult Detalii(int? id)
        {
            var beaturi = _context.Beaturi.SingleOrDefault(c => c.Id == id);

            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(beaturi);
            }

        }
    }
}