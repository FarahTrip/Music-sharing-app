using System.Collections.Generic;
using Trippin_Website.Models;

namespace Trippin_Website.ViewModels
{
    public class PieseStiluriViewModel
    {
        public Piese Piese { get; set; }
        public List<StyleOf> Style { get; set; }
        public List<PieseFileNames> PieseFileNames { get; set; }
    }
}