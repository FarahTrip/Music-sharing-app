using System.Collections.Generic;
using Trippin_Website.Models;

namespace Trippin_Website.ViewModels
{
    public class BeatViewModel
    {
        public List<StyleOf> Styles { get; set; }
        public Beat Beat { get; set; }
        public List<Likes> Likes { get; set; }
        public ApplicationUser User { get; set; }
        public bool HasLiked { get; set; }
        public string PresignedUrl { get; set; }
    }
}