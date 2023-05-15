using System;
using System.Collections.Generic;

namespace Trippin_Website.Models
{
    public class Chat
    {
        public Guid? Id { get; set; }
        public virtual ICollection<ChatMembers> Members { get; set; }

    }
}