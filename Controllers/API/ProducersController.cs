using System;
using System.Web.Http;
using Trippin_Website.Models;
using Trippin_Website.ViewModels;

namespace Trippin_Website.Controllers.API
{
    public class ProducersController : ApiController
    {
        private ApplicationDbContext _context;
        public ProducersController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult AddProducers(ProducersViewModel producersModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();


            foreach (var artistId in producersModel.ProducersIds)
            {
                var whoProducerdTheSong = new WhoProducedTheSong
                {
                    Id = Guid.NewGuid(),
                    ArtistId = artistId,
                    PiesaId = producersModel.BeatId
                };
                _context.WhoProducedTheSong.Add(whoProducerdTheSong);
            }

            _context.SaveChangesAsync();

            return Ok();
        }
    }
}