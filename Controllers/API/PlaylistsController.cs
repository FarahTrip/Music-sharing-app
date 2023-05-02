using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
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

            return Ok(new { success = true });
        }

        [HttpPost]
        [Route("AddToPlaylist/{playListId}/{songId}/{userId}")]
        public async Task<IHttpActionResult> AddToPlaylist(string playListId, string songId, string userId)
        {
            var currentUserId = User.Identity.GetUserId();
            var isAlreadyInThePlaylist = _context.PlaylistContent.Any(c => c.UserId == userId && c.playlistId == c.playlistId && c.playlistId == playListId);

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

            if (isAlreadyInThePlaylist)
                return Ok(new { success = true, alreadyInPlaylist = true });

            _context.PlaylistContent.Add(playlistContent);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, alreadyInPlaylist = false });
        }
    }
}