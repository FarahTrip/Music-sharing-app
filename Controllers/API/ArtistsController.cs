using System;
using System.Web.Http;
using Trippin_Website.Models;
using Trippin_Website.ViewModels;

namespace Trippin_Website.Controllers.API
{
    public class ArtistsController : ApiController
    {
        private ApplicationDbContext _context;
        public ArtistsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult AddArtists(ArtistsViewModel artistsModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();


            foreach (var artistId in artistsModel.ArtistIds)
            {
                var whoIsOnTheSong = new WhoIsOnTheSong
                {
                    Id = Guid.NewGuid(),
                    ArtistId = artistId,
                    PiesaId = artistsModel.PiesaId
                };
                _context.WhoIsOnTheSong.Add(whoIsOnTheSong);
            }

            _context.SaveChangesAsync();

            return Ok();
        }
    }
}