using System;

namespace Trippin_Website.Models
{
    public class PlayList
    {
        public Guid? Id { get; set; }
        public string userId { get; set; }
        public string piesaId { get; set; }
        public string beatId { get; set; }
        public bool IsPrivate { get; set; }

    }
}