using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Trippin_Website.Models;

namespace Trippin_Website.Controllers.API
{
    [Authorize(Roles = "Admin")]
    public class PieseAPIController : ApiController
    {
        private ApplicationDbContext _Context;
        public PieseAPIController()
        {
            _Context = new ApplicationDbContext();
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
        public void DeletePiesa(Guid id, Piese Piesa)
        {
            var piesa = _Context.Piese.FirstOrDefault(c => c.Id == id);

            if (piesa == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _Context.Piese.Remove(piesa);
            _Context.SaveChanges();
        }
    }
}
