using System;

namespace Trippin_Website.DTOS
{
    public class PlayListContentsDTO
    {
        public Guid? Id { get; set; }
        public string playlistId { get; set; }
        public string piesaId { get; set; }
        public string beatId { get; set; }
        public string UserId { get; set; }
        public DateTime? DateAdded { get; set; }
        public string SongName { get; set; }
    }
}