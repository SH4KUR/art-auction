using System.Collections.Generic;
using ArtAuction.Core.Domain.Entities;

namespace ArtAuction.Core.Application.Models
{
    public class AuctionsWithTotalCount
    {
        public IEnumerable<Auction> Auctions { get; set; }
        public int TotalCount { get; set; }
    }
}