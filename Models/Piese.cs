using System;
using System.ComponentModel.DataAnnotations;
using Trippin_Website.Models;

namespace Trippin_Website
{
    public class Piese
    {
        [Key]
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Nu ai introdus numele piesei!")]
        public string Name { get; set; }

        public string Description { get; set; }


        [Required(ErrorMessage = "Nu ai introdus gama piesei!")]
        public string Key { get; set; }

        [Range(30, 200, ErrorMessage = "Oops! Ai introdus un BPM sub 30bpm sau peste 200bpm")]
        public int Bpm { get; set; }

        public int BeatId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }


        public StyleOf Style { get; set; }

        public byte StyleId { get; set; }

        public bool IsBanger { get; set; }
        public string FileName { get; set; }
        public string S3ServerPath { get; set; }
        public string UserId { get; set; }
        public int Likes { get; set; }
        public float FileSize { get; set; }

        public bool IsPublic { get; set; }
        public bool IsJustForMyGroup { get; set; }
        public string Currentprogress { get; set; }

    }
}