using System.Web.Mvc;

namespace Trippin_Website.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PleaseConfirmEmal()
        {
            return View();
        }

    }
}