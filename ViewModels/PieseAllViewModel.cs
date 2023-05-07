using System.Collections.Generic;
using Trippin_Website.DTOS;
using Trippin_Website.Models;

namespace Trippin_Website.ViewModels
{
    public class PieseAllViewModel
    {
        public List<Piese> Piese { get; set; } = new List<Piese>();
        public List<StyleOf> Stiluri { get; set; } = new List<StyleOf>();
        public List<PieseFileNames> PieseFileNames { get; set; } = new List<PieseFileNames>();
        public List<ApplicationUser> Users { get; set; }

        public Likes Likes { get; set; }
        public List<PlayList> PlayLists { get; set; }
        public List<PlayListContentsDTO> PlaylistContent { get; set; }
    }
}
