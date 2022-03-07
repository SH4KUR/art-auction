using System.Collections.Generic;

namespace ArtAuction.Core.Application.DTO
{
    public class AuctionCatalogWithPagingDto
    {
        public IEnumerable<AuctionCatalogDto> Auctions { get; set; }

        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int RowsOnPage { get; set; }
    }
}