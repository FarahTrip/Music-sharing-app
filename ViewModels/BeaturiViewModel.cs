using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using Trippin_Website.Models;

namespace Trippin_Website.ViewModels
{
    public class BeaturiViewModel
    {
        public List<Beat> Beaturi { get; set; }
        public List<StyleOf> Stiluri { get; set; }
        public UserManager<ApplicationUser> UserManager { get; set; }

    }
}