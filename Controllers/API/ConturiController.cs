using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Http;
using Trippin_Website.Models;

namespace Trippin_Website.Controllers.API
{


    public class ConturiController : ApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConturiController()
        {
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }
        // GET: Conturi


        [System.Web.Http.HttpGet]
        public IHttpActionResult GetUser(string id)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var user = _userManager.FindById(id);

            return Ok(user);
        }
    }
}