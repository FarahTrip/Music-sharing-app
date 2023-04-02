using Amazon;
using Amazon.S3;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Trippin_Website.Logic_classes;
using Trippin_Website.Models;

namespace Trippin_Website.Controllers.API
{
    [Authorize(Roles = "Admin, Producer, Artist")]
    public class BeaturiAPIController : ApiController
    {
        // GET: BeaturiAPI

        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public BeaturiAPIController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        public IEnumerable<Beat> GetBeats()
        {
            _context.Beaturi.ToList();
            return _context.Beaturi;
        }
        public Beat GetBeat(Guid id)
        {


            var beat = _context.Beaturi.SingleOrDefault(b => b.IdBun == id);

            if (beat == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            return beat;
        }


        [HttpPost]
        public Beat CreateBeat(Beat beat)
        {
            if (beat == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            beat.Created = DateTime.Now;
            _context.Beaturi.Add(beat);
            _context.SaveChanges();

            return beat;
        }

        [HttpPut]
        public void UpdateBeat(Guid id, Beat beat)
        {
            var beatUpdate = _context.Beaturi.SingleOrDefault(b => b.IdBun == id);

            if (beatUpdate == null)
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);

            beatUpdate.Name = beat.Name;
            beatUpdate.Bpm = beat.Bpm;
            beatUpdate.Description = beat.Description;
            beatUpdate.Key = beat.Key;
            beatUpdate.Modified = DateTime.Now;

            _context.SaveChanges();
        }

        [HttpDelete]
        public IHttpActionResult DeleteBeat(Guid id)
        {
            var fileServerHelper = new AmazonHelper();
            var beat = _context.Beaturi.FirstOrDefault(c => c.IdBun == id);
            var client = new AmazonS3Client(fileServerHelper.AccessId, fileServerHelper.SecretKey, RegionEndpoint.EUNorth1);

            if (beat == null)
                return NotFound();

            var userId = User.Identity.GetUserId();
            var user = _userManager.FindById(userId);

            if (beat.S3ServerPath != null && userId == beat.UserId || User.IsInRole("Admin"))
            {
                client.DeleteObjectAsync(fileServerHelper.BucketName, beat.S3ServerPath);
                user.Quota -= beat.FileSize;

                _userManager.Update(user);
                _context.Beaturi.Remove(beat);
                _context.SaveChanges();

                return Ok();
            }
            else
                return BadRequest();
        }
    }
}