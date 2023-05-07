using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Trippin_Website.Models;

namespace Trippin_Website.Controllers.API
{
    [Authorize]
    public class PlaylistsController : ApiController
    {
        private ApplicationDbContext _context { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        // GET: Playlist
        public PlaylistsController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        }


        [HttpPost]
        [Route("CreatePlaylist/{userId}/{playlistName}")]
        public async Task<IHttpActionResult> CreatePlaylist(string userId, string playlistName)
        {
            var currentUserId = User.Identity.GetUserId();

            if (!ModelState.IsValid || userId == null)
                return BadRequest();
            if (currentUserId != userId)
                return BadRequest();

            var playlist = new PlayList()
            {
                Id = Guid.NewGuid(),
                userId = userId,
                Name = playlistName,
                DateCreated = DateTime.Now
            };

            _context.PlayList.Add(playlist);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, playlistId = playlist.Id });
        }

        [HttpPost]
        [Route("AddToPlaylist/{playListId}/{songId}/{userId}")]
        public async Task<IHttpActionResult> AddToPlaylist(string playListId, string songId, string userId)
        {
            var currentUserId = User.Identity.GetUserId();
            var isAlreadyInThePlaylist = _context.PlaylistContent.Any(c => c.piesaId == songId && c.playlistId == playListId);

            if (!ModelState.IsValid || userId == null)
                return BadRequest();
            if (currentUserId != userId)
                return BadRequest();


            PlaylistContent playlistContent = new PlaylistContent()
            {
                Id = Guid.NewGuid(),
                playlistId = playListId,
                piesaId = songId,
                DateAdded = DateTime.Now,
                UserId = currentUserId
            };

            var song = _context.Piese.SingleOrDefault(c => c.Id.ToString() == playlistContent.piesaId);

            if (song == null)
            {
                return BadRequest("Piesa nu a fost gasita!");
            }

            if (isAlreadyInThePlaylist)
                return Ok(new { success = true, alreadyInPlaylist = true, songName = song.Name });

            try
            {
                _context.PlaylistContent.Add(playlistContent);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, alreadyInPlaylist = false, songName = song.Name });
            }
            catch (Exception ex)
            {
                return Json(new { success = true, alreadyInPlaylist = false, songName = song.Name, exception = ex.Message });
            }



        }

        [HttpDelete]
        [Route("DeletePlaylist/{playListId}/{userId}")]
        public async Task<IHttpActionResult> DeletePlayList(string playListId, string userId)
        {
            var currentUserId = User.Identity.GetUserId();
            var playlist = _context.PlayList.SingleOrDefault(c => c.Id.ToString() == playListId);
            var songsInThatPlaylist = _context.PlaylistContent.Where(c => c.playlistId == playListId);

            if (!ModelState.IsValid || userId == null)
                return BadRequest();
            if (currentUserId != userId)
                return BadRequest();
            if (playlist == null)
                return BadRequest();

            if (songsInThatPlaylist != null)
                _context.PlaylistContent.RemoveRange(songsInThatPlaylist);

            _context.PlayList.Remove(playlist);
            await _context.SaveChangesAsync();


            return Ok(new { success = true });
        }

        [HttpDelete]
        [Route("DeletePlaylist/{playListId}/{songId}/{userId}")]
        public async Task<IHttpActionResult> DeleteContent(string playListId, string songId, string userId)
        {
            var currentUserId = User.Identity.GetUserId();
            var playlist = _context.PlayList.SingleOrDefault(c => c.Id.ToString() == playListId);
            var theSongExist = _context.PlaylistContent.Any(c => c.piesaId == songId && c.playlistId == playListId);

            if (!ModelState.IsValid || userId == null)
                return BadRequest();
            if (currentUserId != userId)
                return BadRequest("Nu esti autorizat sa faci asta!");
            if (playlist == null)
                return BadRequest("PLaylist-ul nu exista!");

            if (theSongExist)
            {
                var song = await _context.PlaylistContent.SingleOrDefaultAsync(c => c.piesaId == songId && c.playlistId == playListId);
                _context.PlaylistContent.Remove(song);
                await _context.SaveChangesAsync();

            }

            return Ok(new { success = true });
        }

    }
}