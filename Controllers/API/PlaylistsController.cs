using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Web.Http;
using Trippin_Website.Models;

namespace Trippin_Website.Controllers.API
{
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
        public IHttpActionResult CrreatePlaylist(Guid userId)
        {

            return Ok();
        }
    }
}