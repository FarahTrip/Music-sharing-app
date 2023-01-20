using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Trippin_Website.Models;

namespace Trippin_Website.Controllers
{
    public class ClientiController : Controller
    {
        private ApplicationDbContext _context;
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public ClientiController()
        {
            _context = new ApplicationDbContext();

        }
        public ActionResult Index()
        {
            var clienti = _context.Clienti.Include(c => c.MembershipType).ToList();
            return View(clienti);
        }

        [Route("Clienti/detalii/{id?}")]
        public ActionResult Detalii(int? id)
        {
            var clienti = _context.Clienti.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id);

            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(clienti);
            }
        }
    }
}