using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
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
            var piesa = _Context.Piese.FirstOrDefault(c => c.Id == id);

            if (piesa == null)
                return NotFound();

            var userId = User.Identity.GetUserId();


            if (piesa.FileName != null && userId == piesa.UserId || User.IsInRole("Admin"))
            {
                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Piese-Uploaded/"), piesa.FileName);
                if ((System.IO.File.Exists(path)))
                {
                    System.IO.File.Delete(path);
                }

                _Context.Piese.Remove(piesa);
                _Context.SaveChanges();

                return Ok();
            }
            else
                return BadRequest();
        }
    }
}
