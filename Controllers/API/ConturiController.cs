using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Trippin_Website.DTOS;
using Trippin_Website.Models;

namespace Trippin_Website.Controllers.API
{

    [Authorize]
    public class ConturiController : ApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ConturiController()
        {
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            _context = new ApplicationDbContext();

        }
        // GET: Conturi


        [System.Web.Http.HttpGet]
        public IHttpActionResult GetUser(string id)
        {
            if (!ModelState.IsValid)
                return NotFound();

            if (id != User.Identity.GetUserId())
                return BadRequest();

            var user = _userManager.FindById(id);

            return Ok(user);
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult GetUsers(string query = null)
        {
            var users = _context.Users.ToList();
            IEnumerable<UsersDTO> preResult;
            List<UsersDTO> result;

            if (!String.IsNullOrWhiteSpace(query))
            {
                preResult = users.Where(c => c.UserName.Contains(query))
                                .Select(c => new UsersDTO { UserName = c.UserName, UserId = c.Id });

                result = preResult.ToList();
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }




    }
}