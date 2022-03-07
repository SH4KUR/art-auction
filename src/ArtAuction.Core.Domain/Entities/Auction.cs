using System;
using System.Collections.Generic;

namespace ArtAuction.Core.Domain.Entities
{
    public class Auction
    {
        public Guid AuctionId { get; set; }
        public int AuctionNumber { get; set; }
        public Guid LotId { get; set; }
        public Lot Lot { get; set; }
        public Guid SellerId { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime StartBillingDateTime { get; set; }
        public DateTime EndBillingDateTime { get; set; }
        public decimal StartPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal? FullPrice { get; set; }
        public decimal BidStep { get; set; }
        public IEnumerable<Bid> Bids { get; set; }
        public IEnumerable<Message> Messages { get; set; }
        public bool IsClosed { get; set; }
        public Guid? CustomerId { get; set; }
    }
}