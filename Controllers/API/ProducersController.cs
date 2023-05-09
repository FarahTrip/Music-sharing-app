using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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

            try
            {
                foreach (var artistId in producersModel.ProducersIds)
                {
                    var whoProducerdTheSong = new WhoProducedTheSong
                    {
                        Id = Guid.NewGuid(),
                        ArtistId = artistId,
                        PiesaId = producersModel.PiesaId
                    };
                    _context.WhoProducedTheSong.Add(whoProducerdTheSong);
                }

                _context.SaveChangesAsync();

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "StackTrace :" + ex.StackTrace);
            }

        }
        [Route("ModificaProduceri")]
        [HttpPost]
        public async Task<IHttpActionResult> ModifyProducersListForSongs(ProducersViewModel ProducersModel)
        {
            var piesa = await _context.Piese.SingleOrDefaultAsync(c => c.Id.ToString() == ProducersModel.PiesaId);
            var piesaProducers = _context.WhoProducedTheSong.Where(c => c.PiesaId == piesa.Id.ToString());

            if (ProducersModel.PiesaId == null)
                return NotFound();

            if (piesa == null)
                return NotFound();
            else
            {
                try
                {
                    _context.WhoProducedTheSong.RemoveRange(piesaProducers);
                    foreach (var artistId in ProducersModel.ProducersIds)
                    {
                        var whoProducedTheSong = new WhoProducedTheSong
                        {
                            Id = Guid.NewGuid(),
                            ArtistId = artistId,
                            PiesaId = ProducersModel.PiesaId
                        };
                        _context.WhoProducedTheSong.Add(whoProducedTheSong);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Json(new { message = ex.Message });
                }

            }

            return Ok();
        }

    }
}