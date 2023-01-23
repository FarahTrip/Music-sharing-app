using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trippin_Website.Models;
using Trippin_Website.ViewModels;

namespace Trippin_Website.Controllers
{
    public class UploadController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult UploadPiesa()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadPiesa(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Piese-Uploaded"), _FileName);
                    file.SaveAs(_path);

                    var FileNameObject = new PieseFileNames()
                    {
                        Name = _FileName
                    };

                    using (_context)
                    {
                        _context.PieseFileNames.Add(FileNameObject);
                        _context.SaveChanges();
                    }
                }
                string _FileName2 = Path.GetFileName(file.FileName);
                ViewBag.Message = $"'{_FileName2}' a fost uploadat cu succes! :)!";

                return View();
            }
            catch
            {
                ViewBag.Message = "Incarcarea a esuat!!";
                return View();
            }
        }
        public ActionResult AddFileNameToDb()
        {
            var FileNames = _context.PieseFileNames.ToList();
            var viewModel = new PieseAndPieseFileNamesViewModel()
            {
                PieseFileNames = FileNames
            };
            return View(viewModel);

        }
        public ActionResult SaveFilesToDb(Piese piese)
        {
            _context.Piese.Add(piese);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}

