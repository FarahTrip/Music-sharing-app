using System.Collections.Generic;

namespace Trippin_Website.Models
{
    public class Artisti
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Piese> Piese { get; set; }

        public int PieseId { get; set; }
    }
}