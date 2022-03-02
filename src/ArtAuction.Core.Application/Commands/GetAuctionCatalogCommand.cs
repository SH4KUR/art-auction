using System.Collections.Generic;
using ArtAuction.Core.Application.DTO;
using ArtAuction.Core.Domain.Enums;
using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class GetAuctionCatalogCommand : IRequest<AuctionCatalogWithPagingDto>
    {
        public GetAuctionCatalogCommand(SortingRule sorting, IEnumerable<string> categories, decimal? minCurrentPrice, 
            decimal? maxCurrentPrice, int pageNumber, int rowsOnPage, bool isClosed)
        {
            Sorting = sorting;
            Categories = categories;
            MinCurrentPrice = minCurrentPrice;
            MaxCurrentPrice = maxCurrentPrice;
            PageNumber = pageNumber;
            RowsOnPage = rowsOnPage;
            IsClosed = isClosed;
        }

        public SortingRule Sorting { get; }
        public IEnumerable<string> Categories { get; }
        public decimal? MinCurrentPrice { get; }
        public decimal? MaxCurrentPrice { get; }
        public int PageNumber { get; }
        public int RowsOnPage { get; }
        public bool IsClosed { get; }
    }
}