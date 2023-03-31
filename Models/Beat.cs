using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trippin_Website.Models
{
    public class Beat
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? IdBun { get; set; }

        [Required(ErrorMessage = "Nu ai introdus numele!")]
        [Display(Name = "Nume")]
        public string Name { get; set; }


        [Display(Description = "Descriere")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Nu ai introdus gama!")]
        public string Key { get; set; }


        [Range(30, 200, ErrorMessage = "Ai introdus un BPM intre 30 si 200!")]
        public int Bpm { get; set; }

        public StyleOf Style { get; set; }

        public byte StyleId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        public bool IsBanger { get; set; }

        [DataType(DataType.Url)]
        public string FileName { get; set; }
        public string S3ServerPath { get; set; }
        public string UserId { get; set; }
        public int Likes { get; set; }

    }
}