using System;
using System.ComponentModel.DataAnnotations;

namespace Trippin_Website.Models
{
    public class Versuri
    {
        public Guid? Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Vers { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public string UserId { get; set; }
    }
}