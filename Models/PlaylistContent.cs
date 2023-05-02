using System;

namespace Trippin_Website.Models
{
    public class PlaylistContent
    {
        public Guid? Id { get; set; }
        public string playlistId { get; set; }
        public string piesaId { get; set; }
        public string beatId { get; set; }
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }

    }
}