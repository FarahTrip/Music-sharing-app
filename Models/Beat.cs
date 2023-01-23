using System;
using System.ComponentModel.DataAnnotations;

namespace Trippin_Website.Models
{
    public class Beat
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nume")]
        public string Name { get; set; }
        [Display(Description = "Descriere")]
        public string Description { get; set; }

        public string Key { get; set; }

        [Required]
        public int Bpm { get; set; }

        public StyleOf Style { get; set; }

        public byte StyleId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }


    }
}