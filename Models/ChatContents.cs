using System;

namespace Trippin_Website.Models
{
    public class ChatContents
    {
        public Guid? Id { get; set; }
        public string Message { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public DateTime SentAt { get; set; }
        public string ChatId { get; set; }
        public string UserName { get; set; }
        public bool IsRead { get; set; }

    }
}