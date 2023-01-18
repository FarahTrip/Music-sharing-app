using System.Collections.Generic;
using System.Web.Mvc;
using Trippin_Website.ViewModels;

namespace Trippin_Website.Controllers
{
    public class PieseController : Controller
    {

        // GET: Piese
        public ActionResult Index()
        {
            var piese = new List<Piese>
            {
                new Piese {Id = 1, Name = "Doi plopi se leagana", Key = "C#", Bpm = 120},
                new Piese {Id = 2,Name = "Lean varsat in poseta", Key = "D", Bpm = 152},
                new Piese {Id = 3,Name = "Decembrie", Key = "A", Bpm = 90}

            };
            var ViewModel = new songbeatartistViewModel
            {
                Piese = piese

            };
            return View(ViewModel);
        }

        [Route("Piese/detalii/{id?}")]
        public ActionResult Detalii(int? id)
        {
            var piese = new List<Piese>
            {
                new Piese {Id = 1, Name = "Doi plopi se leagana", Key = "C#", Bpm = 120},
                new Piese {Id = 2,Name = "Lean varsat in poseta", Key = "D", Bpm = 152},
                new Piese {Id = 3,Name = "Decembrie", Key = "A", Bpm = 90}

            };
            var ViewModel = new songbeatartistViewModel
            {
                Piese = piese

            };

            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(ViewModel);
            }

        }
    }
}