using System;

namespace Trippin_Website.Models
{
    public class Likes
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public Guid? PiesaId { get; set; }
        public Guid? BeatId { get; set; }

    }
}