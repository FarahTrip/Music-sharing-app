using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IHttpActionResult> AddArtists(ArtistsViewModel artistsModel)
        {
            string userId = User.Identity.GetUserId();
            var piesa = await _context.Piese.SingleOrDefaultAsync(c => c.Id.ToString() == artistsModel.PiesaId);

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

            await _context.SaveChangesAsync();

            return Ok();
        }

        [Route("ModificaArtisti")]
        [HttpPost]
        public async Task<IHttpActionResult> ModifyArtistsList(ArtistsViewModel artistsModel)
        {
            var piesa = await _context.Piese.SingleOrDefaultAsync(c => c.Id.ToString() == artistsModel.PiesaId);
            var piesaArtists = _context.WhoIsOnTheSong.Where(c => c.PiesaId == piesa.Id.ToString());

            if (artistsModel.PiesaId == null)
                return NotFound();

            if (piesa == null)
                return NotFound();
            else
            {
                try
                {
                    _context.WhoIsOnTheSong.RemoveRange(piesaArtists);
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
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Json(new { message = ex.Message });
                }

            }

            return Ok(new { raspuns = "Totul a functionat corect!" });
        }
    }
}