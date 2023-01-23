using System;
using System.ComponentModel.DataAnnotations;
using Trippin_Website.Models;

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
        public int BeatIDForLinking { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public StyleOf Style { get; set; }

        public byte StyleId { get; set; }
        public bool IsBanger { get; set; }

        public PieseFileNames PiesaFileName { get; set; }
        public int? PiesaFileNameId { get; set; }

    }
}