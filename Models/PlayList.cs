using System;

namespace Trippin_Website.Models
{
    public class PlayList
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string userId { get; set; }
        public bool IsPrivate { get; set; }

        public DateTime? DateCreated { get; set; }

    }
}