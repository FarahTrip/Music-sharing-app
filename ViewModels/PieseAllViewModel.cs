using System.Collections.Generic;
using Trippin_Website.Models;

namespace Trippin_Website.ViewModels
{
    public class PieseAllViewModel
    {
        public List<Piese> Piese { get; set; } = new List<Piese>();
        public List<StyleOf> Stiluri { get; set; } = new List<StyleOf>();
        public List<PieseFileNames> PieseFileNames { get; set; } = new List<PieseFileNames>();

    }
}