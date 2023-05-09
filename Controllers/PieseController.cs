using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Trippin_Website.DTOS;
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
            var UserId = User.Identity.GetUserId();
            var playLists = _context.PlayList.Where(c => c.userId == UserId).OrderByDescending(c => c.DateCreated).ToList();

            var playlistContents = _context.PlaylistContent.Where(c => c.UserId == UserId).ToList();
            var playlistContentsDTO = new List<PlayListContentsDTO>();

            if (playlistContents != null)
            {
                foreach (var content in playlistContents)
                {
                    var song = piese.SingleOrDefault(c => c.Id.ToString() == content.piesaId);
                    if (song != null)
                    {
                        playlistContentsDTO.Add(new PlayListContentsDTO()
                        {
                            Id = content.Id,
                            UserId = UserId,
                            playlistId = content.playlistId,
                            piesaId = content.piesaId,
                            beatId = content.beatId,
                            DateAdded = content.DateAdded,
                            SongName = song.Name,
                        });
                    }
                }
            }

            var pieseAllViewModel = new PieseAllViewModel()
            {
                Piese = piese,
                Stiluri = Stiluri,
                Users = users,
                PlayLists = playLists,
                PlaylistContent = playlistContentsDTO
            };
            var pieseIndexViewModel = new PieseIndexViewModel()
            {
                PieseAllViewModel = pieseAllViewModel,
                UserManager = _userManager,
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
            ViewBag.GuidId = Guid.NewGuid();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Producer, Artist")]
        public async Task<ActionResult> Creeaza(Piese piese, HttpPostedFileBase file, Guid piesaId)
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

            if (IsAudio(file) && file.ContentLength > 0)
                try
                {
                    var verifyIfIsInQuota = user.Quota + file.ContentLength;

                    if (!User.IsInRole("Admin") && verifyIfIsInQuota > user.FileUploadHardLimit)
                    {
                        ViewBag.Message = "Quota de upload a fost depasita. Nu vei putea uploada noi fisiere decat daca vei sterge altele vechi sau iti vei schimba planul cu unul platit.";
                        return View("AdaugaNou", PieseModel);
                    }
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

                    piese.Id = piesaId;
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
            var userId = User.Identity.GetUserId();

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
            var WhoIsOnTheSong = _context.WhoIsOnTheSong.Where(m => m.PiesaId == piesa.Id.ToString()).ToList();
            var WhoProducedTheSong = _context.WhoProducedTheSong.Where(m => m.PiesaId == piesa.Id.ToString()).ToList();

            var Model = new PieseViewModel()
            {
                Piese = piesa,
                Style = stiluri,
                WhoIsOnTheSong = WhoIsOnTheSong,
                WhoProducedTheSong = WhoProducedTheSong

            };

            if (piesa.UserId != userId && !User.IsInRole("Admin"))
                return RedirectToAction("Index");

            return View(Model);
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Producer, Artist")]
        public ActionResult ModificaSaved(PieseViewModel PiesaModel)
        {
            var userId = User.Identity.GetUserId();

            var CurrentDateTime = DateTime.Now;
            var PieseInDb = _context.Piese.Single(c => c.Id == PiesaModel.Piese.Id);

            if (PieseInDb.UserId != userId && !User.IsInRole("Admin"))
                return RedirectToAction("Index", "Piese");


            PieseInDb.Name = PiesaModel.Piese.Name;
            PieseInDb.Key = PiesaModel.Piese.Key;
            PieseInDb.StyleId = PiesaModel.Piese.StyleId;
            PieseInDb.Description = PiesaModel.Piese.Description;
            PieseInDb.Bpm = PiesaModel.Piese.Bpm;
            PieseInDb.IsBanger = PiesaModel.Piese.IsBanger;
            PieseInDb.DateModified = CurrentDateTime;
            PieseInDb.Currentprogress = PiesaModel.Piese.Currentprogress;
            PieseInDb.IsJustForMyGroup = PiesaModel.Piese.IsJustForMyGroup;
            PieseInDb.IsPublic = PiesaModel.Piese.IsPublic;

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