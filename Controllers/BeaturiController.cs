﻿using Amazon;
using Amazon.S3;
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
    public class BeaturiController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public BeaturiController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }
        // GET: Piese
        public ActionResult Index()
        {
            var viewModel = new BeaturiViewModel()
            {
                Beaturi = _context.Beaturi.OrderByDescending(c => c.Created).ToList(),
                Stiluri = _context.StyleOf.ToList(),
                UserManager = _userManager
            };
            return View(viewModel);
        }

        [Route("Beaturi/detalii/{id?}")]
        public ActionResult Detalii(Guid? id)
        {
            var beaturi = _context.Beaturi.Include(c => c.Style).SingleOrDefault(c => c.IdBun == id);
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var viewModel = new BeatViewModel
            {
                Beat = beaturi,
                Styles = _context.StyleOf.ToList()
            };
            return View(viewModel);
        }

        public ActionResult Edit()
        {
            var beaturi = _context.Beaturi.ToList();
            return View(beaturi);
        }


        public ActionResult AdaugaNou()
        {

            var StiluriList = _context.StyleOf.ToList();
            var viewModel = new BeatViewModel
            {
                Styles = StiluriList,
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Creeaza(Beat Beat, HttpPostedFileBase file)
        {
            var fileServerHelper = new AmazonHelper();
            var BeaturiModel = new BeatViewModel()
            {
                Beat = Beat,
                Styles = _context.StyleOf.ToList(),
            };

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Modelul nu este valid! Daca problema persista contactati suportul!";
                return View("AdaugaNou", BeaturiModel);
            }

            fileServerHelper.UserFolder(User.Identity.GetUserId());
            var userId = User.Identity.GetUserId();


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

                    Beat.IdBun = Guid.NewGuid();
                    Beat.UserId = User.Identity.GetUserId();
                    Beat.S3ServerPath = key;
                    _context.Beaturi.Add(Beat);
                    _context.SaveChanges();

                    return RedirectToAction("Index", "Beaturi");
                }
                catch (AmazonS3Exception ex)
                {
                    var errorMessage = ex.Message;
                    var statusCode = ex.StatusCode;

                    ViewBag.Message = $@"A aparut o eroare la urcarea pe server. Status code : {statusCode} : Mesaj : {errorMessage}.
Daca eroarea persista va rog sa anuntati suportul din pagina de contanct.";
                    return View("AdaugaNou", BeaturiModel);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = $@"A aparut o neasteptata. Eroare : {ex.Message}. Stack : {ex.StackTrace}. 
Daca eroarea persista va rog sa anuntati suportul din pagina de contanct.";
                    return View("AdaugaNou", BeaturiModel);
                }
            else
            {
                ViewBag.Message = "Te rog seleteaza un fisier .wav, .mp3 sau .ogg!!";
                return View("AdaugaNou", BeaturiModel);
            }

        }
        public ActionResult Modifica(Beat Beat)
        {
            var CurrentDateTime = DateTime.Now;
            var BeatInDb = _context.Beaturi.Single(c => c.IdBun == Beat.IdBun);
            BeatInDb.Name = Beat.Name;
            BeatInDb.Key = Beat.Key;
            BeatInDb.Style = Beat.Style;
            BeatInDb.Description = Beat.Description;
            BeatInDb.Bpm = Beat.Bpm;
            BeatInDb.Modified = CurrentDateTime;
            _context.SaveChanges();
            return RedirectToAction("Index", "Beaturi");
        }

        public ActionResult EditBeat(Guid Id)
        {
            var beat = _context.Beaturi.SingleOrDefault(c => c.IdBun == Id);
            if (beat == null)
            {
                return HttpNotFound();
            }
            else
            {
                var viewModel = new BeatViewModel
                {
                    Beat = beat,
                    Styles = _context.StyleOf.ToList()
                };
                return View(viewModel);

            }
        }

        private bool IsAudio(HttpPostedFileBase file)
        {
            if (file == null) { return false; }

            string[] extensions = new string[] { "wav", "mp3", "ogg" };

            return extensions.Any(i => file.FileName.EndsWith(i, StringComparison.OrdinalIgnoreCase));
        }
    }
}
