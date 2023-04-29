using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Trippin_Website.Logic_classes;
using Trippin_Website.Models;

namespace Trippin_Website.Controllers.API
{
    [Authorize]
    public class MusicPlayerController : ApiController
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MusicPlayerController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }
        public async Task<IHttpActionResult> GetAudio(Guid audioId/*, string audioSongOrBeat*/)
        {
            var helper = new AmazonHelper();
            AmazonS3Client client = new AmazonS3Client(helper.AccessId, helper.SecretKey, RegionEndpoint.EUNorth1);
            var piesa = await _context.Piese.SingleOrDefaultAsync(c => c.Id == audioId);

            var artisti = _context.WhoIsOnTheSong
                .Where(c => c.PiesaId == audioId.ToString())
                .Join(_userManager.Users,
                    whoIsOnTheSong => whoIsOnTheSong.ArtistId,
                    user => user.Id,
                    (whoIsOnTheSong, user) => user.UserName)
                .ToList();

            artisti.Reverse();

            var userId = User.Identity.GetUserId();
            var key = piesa.S3ServerPath;

            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
            {
                BucketName = helper.BucketName,
                Expires = DateTime.Now.AddMinutes(20),
                Key = key
            };

            string path = client.GetPreSignedURL(request);
            bool HasLiked = _context.Likes.Any(c => c.UserId == userId && c.PiesaId == audioId);

            if (artisti.Any())
                return Json(new { title = piesa.Name, src = path, artists = artisti, hasLiked = HasLiked, id = piesa.Id });

            return Json(new { title = piesa.Name, src = path, hasLiked = HasLiked, piesa.Id });
        }
    }
}