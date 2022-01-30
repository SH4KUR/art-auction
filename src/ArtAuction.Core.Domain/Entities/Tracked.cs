using System;

namespace ArtAuction.Core.Domain.Entities
{
    public class Tracked
    {
        public Guid UserId { get; set; }
        public Guid AuctionId { get; set; }
    }
}