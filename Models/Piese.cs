using System.ComponentModel.DataAnnotations;

namespace Trippin_Website
{
    public class Piese
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Key { get; set; }
        public int Bpm { get; set; }
        public string Style { get; set; }

        public int BeatIDForLinking { get; set; }

    }
}