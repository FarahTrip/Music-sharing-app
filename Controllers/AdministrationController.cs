using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Web.Mvc;
using Trippin_Website.ViewModels;

namespace Trippin_Website.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        // GET: Administration
        public AdministrationController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [HttpPost]
        public async Task<ActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole { Name = model.RoleName };
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(model);
        }
    }
}