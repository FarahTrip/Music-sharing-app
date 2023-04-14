using System.Linq;
using System.Web.Mvc;
using Trippin_Website.Models;

namespace Trippin_Website.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ApplicationDbContext _context;
        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            ViewBag.Piese = _context.Piese.Count();
            ViewBag.Beaturi = _context.Beaturi.Count();
            ViewBag.Conturi = _context.Users.Count();
            return View();
        }

        public ActionResult PleaseConfirmEmal()
        {
            return View();
        }

    }
}