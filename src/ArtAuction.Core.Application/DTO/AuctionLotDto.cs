using System.Collections.Generic;

namespace ArtAuction.Core.Application.DTO
{
    public class AuctionLotDto
    {
        public AuctionCatalogDto AuctionLot { get; set; }
        public IEnumerable<MessageDto> Messages { get; set; }
        public IEnumerable<BidDto> Bids { get; set; }
    }
}