using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.DTO;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Domain.Entities;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class GetAuctionListCommandHandler : IRequestHandler<GetAuctionListCommand, AuctionCatalogWithPagingDto>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IUserRepository _userRepository;
        
        public GetAuctionListCommandHandler(IAuctionRepository auctionRepository, IUserRepository userRepository)
        {
            _auctionRepository = auctionRepository;
            _userRepository = userRepository;
        }

        public async Task<AuctionCatalogWithPagingDto> Handle(GetAuctionListCommand request, CancellationToken cancellationToken)
        {
            var auctionsWithPaging = await _auctionRepository.GetAuctionsAsync(
                request.Sorting,
                request.Categories,
                request.MinCurrentPrice,
                request.MaxCurrentPrice,
                request.PageNumber,
                request.RowsOnPage,
                request.IsClosed);
            
            return new AuctionCatalogWithPagingDto
            { 
                Auctions = MapAuctions(auctionsWithPaging.Auctions),
                
                TotalCount = auctionsWithPaging.TotalCount,
                CurrentPage = auctionsWithPaging.CurrentPage,
                RowsOnPage = auctionsWithPaging.RowsOnPage
            };
        }

        private IEnumerable<AuctionCatalogDto> MapAuctions(IEnumerable<Auction> auctions)
        {
            return auctions.Select(auction => new AuctionCatalogDto
            {
                AuctionNumber = auction.AuctionNumber,
                IsClosed = auction.IsClosed,
                    
                CreationDateTime = auction.CreationDateTime,
                StartBillingDateTime = auction.StartBillingDateTime,
                EndBillingDateTime = auction.EndBillingDateTime,
                    
                StartPrice = auction.StartPrice,
                CurrentPrice = auction.CurrentPrice,
                FullPrice = auction.FullPrice,
                BidStep = auction.BidStep,
                BidsCount = auction.Bids.Count(),
                    
                SellerLogin = _userRepository.GetUser(auction.SellerId).Login,
                CustomerLogin = (auction.CustomerId == null) ? string.Empty : _userRepository.GetUser(auction.CustomerId.Value).Login,
                    
                CategoryName = auction.Lot.Category.Name,
                LotName = auction.Lot.Name,
                PaintingDate = auction.Lot.PaintingDate,
                Photo = auction.Lot.Photo,
                Description = auction.Lot.Description
            });
        }
    }
}