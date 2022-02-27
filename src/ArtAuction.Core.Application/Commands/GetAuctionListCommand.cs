using System.Collections.Generic;
using ArtAuction.Core.Application.DTO;
using ArtAuction.Core.Domain.Enums;
using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class GetAuctionListCommand : IRequest<AuctionCatalogWithPagingDto>
    {
        public SortingRule Sorting { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public decimal? MinCurrentPrice { get; set; }
        public decimal? MaxCurrentPrice { get; set; }
        public int PageNumber { get; set; }
        public int RowsOnPage { get; set; }
        public bool IsClosed { get; set; }
    }
}