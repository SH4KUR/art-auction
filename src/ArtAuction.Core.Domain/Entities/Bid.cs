using System;

namespace ArtAuction.Core.Domain.Entities
{
    public class Bid
    {
        public Guid BidId { get; set; }
        public Guid UserId { get; set; }
        public Guid AuctionId { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Sum { get; set; }
    }
}