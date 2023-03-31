using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Trippin_Website.Models;

namespace Trippin_Website.Controllers.API
{
    [Authorize(Roles = "Admin, Producer")]
    public class BeaturiAPIController : ApiController
    {
        // GET: BeaturiAPI

        private ApplicationDbContext _context;
        public BeaturiAPIController()
        {
            _context = new ApplicationDbContext();
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
            var beatToDelete = _context.Beaturi.SingleOrDefault(c => c.IdBun == id);

            if (beatToDelete == null)
                return NotFound();

            _context.Beaturi.Remove(beatToDelete);
            _context.SaveChanges();

            return Ok();
        }
    }
}