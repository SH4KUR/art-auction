using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.DTO;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace ArtAuction.Core.Application.Handlers
{
    public class GetLastAuctionsCommandHandler : IRequestHandler<GetLastAuctionsCommand, IEnumerable<AuctionCatalogDto>>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public GetLastAuctionsCommandHandler(IAuctionRepository auctionRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            _auctionRepository = auctionRepository;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<IEnumerable<AuctionCatalogDto>> Handle(GetLastAuctionsCommand request, CancellationToken cancellationToken)
        {
            var lastAuctionsHomePageCount = Convert.ToInt32(_configuration["App:LastAuctionsHomePageCount"]);
            
            var auctions = await _auctionRepository.GetLastAuctions(lastAuctionsHomePageCount);
            return auctions.Select(MapAuctionCatalog);
        }

        private AuctionCatalogDto MapAuctionCatalog(Auction auction)
        {
            return new()
            {
                AuctionNumber = auction.AuctionNumber,
                IsClosed = auction.IsClosed,
                IsVip = auction.IsVip,

                CreationDateTime = auction.CreationDateTime,
                StartBillingDateTime = auction.StartBillingDateTime,
                EndBillingDateTime = auction.EndBillingDateTime,

                StartPrice = auction.StartPrice,
                CurrentPrice = auction.CurrentPrice,
                FullPrice = auction.FullPrice,
                BidStep = auction.BidStep,
                BidsCount = auction.Bids.Count(),

                SellerLogin = _userRepository.GetUser(auction.SellerId).Login,
                CustomerLogin = (auction.CustomerId == null)
                    ? string.Empty
                    : _userRepository.GetUser(auction.CustomerId.Value).Login,

                CategoryName = auction.Lot.Category.Name,
                LotName = auction.Lot.Name,
                PaintingDate = auction.Lot.PaintingDate,
                Photo = auction.Lot.Photo,
                Description = auction.Lot.Description
            };
        }
    }
}