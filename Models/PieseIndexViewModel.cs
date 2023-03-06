using Microsoft.AspNet.Identity;
using Trippin_Website.ViewModels;

namespace Trippin_Website.Models
{
    public class PieseIndexViewModel
    {
        public PieseAllViewModel PieseAllViewModel { get; set; }
        public UserManager<ApplicationUser> UserManager { get; set; }
    }
}