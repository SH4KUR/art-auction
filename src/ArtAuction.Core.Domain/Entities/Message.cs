using System;

namespace ArtAuction.Core.Domain.Entities
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public Guid UserId { get; set; }
        public Guid AuctionId { get; set; }
        public DateTime DateTime { get; set; }
        public string MessageText { get; set; }
        public bool IsAdmin { get; set; }
    }
}