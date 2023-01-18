using System.Collections.Generic;
using Trippin_Website.Models;

namespace Trippin_Website.ViewModels
{
    public class songbeatartistViewModel
    {
        public List<Piese> Piese { get; set; } = new List<Piese>();
        public List<Beat> Beaturi { get; set; } = new List<Beat>();
        public List<Artisti> Artisti { get; set; } = new List<Artisti>();
    }
}