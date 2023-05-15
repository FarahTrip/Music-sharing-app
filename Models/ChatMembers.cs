using System;

namespace Trippin_Website.Models
{
    public class ChatMembers
    {
        public Guid? Id { get; set; }
        public string MemberId { get; set; }
        public string ChatId { get; set; }
        public virtual Chat Chat { get; set; }
    }
}