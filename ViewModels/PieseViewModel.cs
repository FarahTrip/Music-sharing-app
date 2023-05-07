using System.Collections.Generic;
using Trippin_Website.Models;

namespace Trippin_Website.ViewModels
{
    public class PieseViewModel
    {
        public Piese Piese { get; set; }
        public List<StyleOf> Style { get; set; }
        public List<PieseFileNames> PieseFileNames { get; set; }
        public List<Likes> Likes { get; set; }
        public ApplicationUser User { get; set; }
        public bool HasLiked { get; set; }
        public string PresignedUrl { get; set; }
        public List<WhoIsOnTheSong> WhoIsOnTheSong { get; set; }
        public List<WhoProducedTheSong> WhoProducedTheSong { get; set; }
    }
}