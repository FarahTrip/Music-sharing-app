using Amazon;
using Amazon.S3;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
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
        public IEnumerable<Piese> GetPiese()
        {
            var piese = _Context.Piese.ToList();
            return piese;
        }
        public Piese GetPiesa(Guid id)
        {
            var piesa = _Context.Piese.FirstOrDefault(c => c.Id == id);

            if (piesa == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return piesa;
        }

        [HttpPost]
        public IHttpActionResult CreatePiesa(Piese PiesaPassed)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            PiesaPassed.DateCreated = DateTime.Now;
            _Context.Piese.Add(PiesaPassed);
            _Context.SaveChanges();

            PiesaPassed.Id = PiesaPassed.Id;
            return Created(new Uri(Request.RequestUri + "/" + PiesaPassed.Id), PiesaPassed);
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
    }
}
