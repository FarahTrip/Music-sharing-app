using System.Collections.Generic;
using Trippin_Website.Models;

namespace Trippin_Website.ViewModels
{
    public class User_Content_ViewModel
    {
        public ApplicationUser User { get; set; }
        public List<Piese> Piese { get; set; }
        public Versuri Vers { get; set; }
        public List<Versuri> Versuri { get; set; }
        public List<Beat> Beaturi { get; set; }

    }
}