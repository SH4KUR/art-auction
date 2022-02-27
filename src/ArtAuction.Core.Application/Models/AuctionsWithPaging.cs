using System.Collections.Generic;
using ArtAuction.Core.Domain.Entities;

namespace ArtAuction.Core.Application.Models
{
    public class AuctionsWithPaging
    {
        public IEnumerable<Auction> Auctions { get; set; }
        
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int RowsOnPage { get; set; }
    }
}