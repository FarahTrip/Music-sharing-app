using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Trippin_Website.Logic_classes;
using Trippin_Website.Models;
using Trippin_Website.ViewModels;

namespace Trippin_Website.Controllers
{
    public class PieseController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public PieseController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }
        // GET: Piese

        [Authorize(Roles = "Admin, Producer, Artist")]
        public ActionResult Index()
        {
            var piese = _context.Piese.OrderByDescending(c => c.DateCreated).ToList();
            var Stiluri = _context.StyleOf.ToList();
            var users = _userManager.Users.ToList();
            var pieseAllViewModel = new PieseAllViewModel()
            {
                Piese = piese,
                Stiluri = Stiluri,
                Users = users
            };
            var pieseIndexViewModel = new PieseIndexViewModel()
            {
                PieseAllViewModel = pieseAllViewModel,
                UserManager = _userManager
            };

            return View(pieseIndexViewModel);
        }

        [AllowAnonymous]
        [Route("Piese/detalii/{id?}")]
        public async Task<ActionResult> Detalii(Guid? id)
        {
            var piese = _context.Piese.Include(c => c.Style).SingleOrDefault(c => c.Id == id);
            var user = _userManager.FindById(piese.UserId);
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var likes = _context.Likes.Where(c => c.PiesaId == id).ToList();

            bool hasLiked = false;
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                hasLiked = _context.Likes.Any(l => l.UserId == userId && l.PiesaId == piese.Id);
            }

            var path = await GetAudioAsync((Guid)piese.Id);
            var viewModel = new PieseViewModel
            {
                Piese = piese,
                Style = _context.StyleOf.ToList(),
                Likes = _context.Likes.Where(c => c.PiesaId == id).ToList(),
                HasLiked = hasLiked,
                PresignedUrl = path
            };

            ViewBag.Likes = likes.Count;

            return View(viewModel);
        }

        [AllowAnonymous]
        public async Task<string> GetAudioAsync(Guid id)
        {
            var helper = new AmazonHelper();
            AmazonS3Client client = new AmazonS3Client(helper.AccessId, helper.SecretKey, RegionEndpoint.EUNorth1);
            var piesa = await _context.Piese.SingleOrDefaultAsync(c => c.Id == id);

            var userId = User.Identity.GetUserId();
            var key = piesa.S3ServerPath;

            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
            {
                BucketName = helper.BucketName,
                Expires = DateTime.Now.AddMinutes(10),
                Key = key
            };

            string path = client.GetPreSignedURL(request);

            return path;
        }

        [Authorize(Roles = "Admin, Producer, Artist")]
        public ActionResult AdaugaNou()
        {
            var Stiluri = _context.StyleOf.ToList();
            var viewModel = new PieseViewModel
            {
                Style = Stiluri
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Producer, Artist")]
        public async Task<ActionResult> Creeaza(Piese piese, HttpPostedFileBase file)
        {
            var fileServerHelper = new AmazonHelper();
            var PieseModel = new PieseViewModel()
            {
                Piese = piese,
                Style = _context.StyleOf.ToList(),
            };

            if (!ModelState.IsValid)
            {
                return View("AdaugaNou", PieseModel);
            }

            fileServerHelper.UserFolder(User.Identity.GetUserId());
            var userId = User.Identity.GetUserId();
            var user = _userManager.FindById(userId);


            var verifyIfIsInQuota = user.Quota + file.ContentLength;

            if (!User.IsInRole("Admin") && verifyIfIsInQuota > user.FileUploadHardLimit)
            {
                ViewBag.Message = "Quota de upload a fost depasita. Nu vei putea uploada noi fisiere decat daca vei sterge altele vechi sau iti vei schimba planul cu unul platit.";
                return View("AdaugaNou", PieseModel);
            }


            if (IsAudio(file) && file.ContentLength > 0)
                try
                {
                    var key = $"Users-Files/{userId}/{file.FileName}";

                    using (var amazonS3client = new AmazonS3Client(fileServerHelper.AccessId, fileServerHelper.SecretKey, RegionEndpoint.EUNorth1))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            file.InputStream.CopyTo(memoryStream);
                            var request = new TransferUtilityUploadRequest
                            {
                                InputStream = memoryStream,
                                Key = key,
                                BucketName = fileServerHelper.BucketName,
                                ContentType = file.ContentType
                            };

                            var transferUtility = new TransferUtility(amazonS3client);
                            await transferUtility.UploadAsync(request);
                        }
                    }
                    ViewBag.Message = "Fisierul a fost incarcat cu succes!";

                    piese.Id = Guid.NewGuid();
                    piese.UserId = User.Identity.GetUserId();
                    piese.S3ServerPath = key;
                    piese.FileSize = file.ContentLength;
                    piese.FileName = file.FileName;
                    user.Quota += file.ContentLength;
                    _context.Piese.Add(piese);
                    _context.SaveChanges();
                    await _userManager.UpdateAsync(user);

                    return RedirectToAction("Index", "Piese");
                }
                catch (AmazonS3Exception ex)
                {
                    var errorMessage = ex.Message;
                    var statusCode = ex.StatusCode;

                    ViewBag.Message = $@"A aparut o eroare la urcarea pe server. Status code : {statusCode} : Mesaj : {errorMessage}.
Daca eroarea persista va rog sa anuntati suportul din pagina de contanct.";
                    return View("AdaugaNou", PieseModel);
                }
                catch
                {
                    ViewBag.Message = "Te rog selecteaza un fisier!!";
                    return View("AdaugaNou", PieseModel);
                }
            else
            {
                ViewBag.Message = "Te rog seleteaza un fisier .wav, .mp3 sau .ogg!!";
                return View("AdaugaNou", PieseModel);
            }
        }

        [Authorize(Roles = "Admin, Producer, Artist")]
        public ActionResult Modifica()
        {
            var piese = _context.Piese.ToList();
            var Stiluri = _context.StyleOf.ToList();
            var pieseModel = new PieseAllViewModel()
            {
                Piese = piese,
                Stiluri = Stiluri,
            };

            return View(pieseModel);
        }

        [Authorize(Roles = "Admin, Producer, Artist")]
        public ActionResult ModificaPiesa(Guid id)
        {
            if (!ModelState.IsValid)
            {
                var piesaForValidation = _context.Piese.SingleOrDefault(c => c.Id == id);
                var ModelForValidation = new PieseViewModel
                {
                    Piese = piesaForValidation,
                    Style = _context.StyleOf.ToList()
                };
                return View(ModelForValidation);
            }
            var piesa = _context.Piese.SingleOrDefault(c => c.Id == id);
            var stiluri = _context.StyleOf.ToList();
            var Model = new PieseViewModel()
            {
                Piese = piesa,
                Style = stiluri
            };
            return View(Model);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Producer, Artist")]
        public ActionResult ModificaSaved(PieseViewModel PiesaModel)
        {
            var CurrentDateTime = DateTime.Now;
            var PieseInDb = _context.Piese.Single(c => c.Id == PiesaModel.Piese.Id);
            PieseInDb.Name = PiesaModel.Piese.Name;
            PieseInDb.Key = PiesaModel.Piese.Key;
            PieseInDb.StyleId = PiesaModel.Piese.StyleId;
            PieseInDb.Description = PiesaModel.Piese.Description;
            PieseInDb.Bpm = PiesaModel.Piese.Bpm;
            PieseInDb.IsBanger = PiesaModel.Piese.IsBanger;
            PieseInDb.DateModified = CurrentDateTime;
            _context.SaveChanges();
            return RedirectToAction("Index", "Piese");
        }
        private bool IsAudio(HttpPostedFileBase file)
        {
            if (file == null) { return false; }

            string[] extensions = new string[] { "wav", "mp3", "ogg" };

            return extensions.Any(i => file.FileName.EndsWith(i, StringComparison.OrdinalIgnoreCase));
        }

    }
}