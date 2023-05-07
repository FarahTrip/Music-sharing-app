﻿using Amazon;
using Amazon.S3;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Routing;
using Trippin_Website.Logic_classes;
using Trippin_Website.Models;

namespace Trippin_Website.Controllers.API
{
    [Authorize]
    public class PieseAPIController : ApiController
    {
        private ApplicationDbContext _Context;
        private readonly UserManager<ApplicationUser> _userManager;
        public PieseAPIController()
        {
            _Context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        [HttpGet]
        [Route("API/GetAll")]
        public IEnumerable<string> GetPiese()
        {
            var piese = _Context.Piese.OrderByDescending(c => c.DateCreated).Select(c => c.Id.ToString()).ToList();
            return piese;
        }
        [HttpGet]
        public Piese GetPiesa(Guid id)
        {
            var piesa = _Context.Piese.FirstOrDefault(c => c.Id == id);

            if (piesa == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return piesa;
        }


        [HttpPut]
        public void UpdatePiesa(Guid id, Piese piese)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var PiesaInDb = _Context.Piese.FirstOrDefault(c => c.Id == id);

            if (PiesaInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            PiesaInDb.Name = piese.Name;
            PiesaInDb.Bpm = piese.Bpm;
            PiesaInDb.Description = piese.Description;
            PiesaInDb.DateModified = DateTime.Now;
            PiesaInDb.Key = piese.Key;

            _Context.SaveChanges();
        }


        [HttpDelete]
        public IHttpActionResult DeletePiesa(Guid id)
        {
            var fileServerHelper = new AmazonHelper();
            var piesa = _Context.Piese.FirstOrDefault(c => c.Id == id);
            var client = new AmazonS3Client(fileServerHelper.AccessId, fileServerHelper.SecretKey, RegionEndpoint.EUNorth1);

            if (piesa == null)
                return NotFound();

            var userId = User.Identity.GetUserId();
            var user = _userManager.FindById(userId);
            var likes = _Context.Likes.Where(c => c.UserId == userId);
            var artistsOnSongs = _Context.WhoIsOnTheSong.Where(c => c.PiesaId == id.ToString());
            var producersOnSongs = _Context.WhoProducedTheSong.Where(c => c.PiesaId == id.ToString());

            if (likes.Any())
                _Context.Likes.RemoveRange(likes);

            if (artistsOnSongs.Any())
                _Context.WhoIsOnTheSong.RemoveRange(artistsOnSongs);

            if (producersOnSongs.Any())
                _Context.WhoProducedTheSong.RemoveRange(producersOnSongs);

            if (piesa.S3ServerPath != null && userId == piesa.UserId || User.IsInRole("Admin"))
            {
                client.DeleteObjectAsync(fileServerHelper.BucketName, piesa.S3ServerPath);
                user.Quota -= piesa.FileSize;

                _userManager.Update(user);
                _Context.Piese.Remove(piesa);
                _Context.SaveChanges();

                return Ok();
            }
            else
                return BadRequest();
        }


        [HttpGet]
        [Route("GetContribuitors/{Id}")]
        public IHttpActionResult GetContribuitors(string Id)
        {
            var WhoIsOnTheSong = _Context.WhoIsOnTheSong
                .Join(_Context.Users, w => w.ArtistId, u => u.Id, (w, u) => new { w, u })
                .Where(m => m.w.PiesaId == Id)
                .Select(m => new
                {
                    Id = m.w.Id,
                    PiesaId = m.w.PiesaId,
                    UserId = m.w.ArtistId,
                    UserName = m.u.UserName
                })
                .ToList();
            var WhoProducedTheSong = _Context.WhoProducedTheSong.Where(m => m.PiesaId == Id).ToList();

            if (Id == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(new { artistiFetch = WhoIsOnTheSong, produceriFetch = WhoIsOnTheSong });
        }
    }
}

