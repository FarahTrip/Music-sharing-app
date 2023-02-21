using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Trippin_Website.Models;

namespace Trippin_Website.Controllers.API
{
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
        public Beat GetBeat(int id)
        {


            var beat = _context.Beaturi.SingleOrDefault(b => b.Id == id);

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
        public void UpdateBeat(int id, Beat beat)
        {
            var beatUpdate = _context.Beaturi.SingleOrDefault(b => b.Id == id);

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
        public IHttpActionResult DeleteBeat(int id)
        {
            var beatToDelete = _context.Beaturi.SingleOrDefault(c => c.Id == id);

            if (beatToDelete == null)
                return NotFound();

            _context.Beaturi.Remove(beatToDelete);
            _context.SaveChanges();

            return Ok();
        }
    }
}