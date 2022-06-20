using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.DTO;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Domain.Entities;
using ArtAuction.Core.Domain.Enums;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class GetUserAuctionsCommandHandler : IRequestHandler<GetUserAuctionsCommand, IEnumerable<AuctionCatalogDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuctionRepository _auctionRepository;

        public GetUserAuctionsCommandHandler(IUserRepository userRepository, IAuctionRepository auctionRepository)
        {
            _userRepository = userRepository;
            _auctionRepository = auctionRepository;
        }
        
        public async Task<IEnumerable<AuctionCatalogDto>> Handle(GetUserAuctionsCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(request.UserLogin);

            var auctions = (user.Role == UserRole.Customer) 
                ? MapAuctions(await _auctionRepository.GetCustomerAuctionsAsync(user.UserId), customerLogin: user.Login) 
                : MapAuctions(await _auctionRepository.GetSellerAuctionsAsync(user.UserId), sellerLogin: user.Login);

            return auctions;
        }

        private IEnumerable<AuctionCatalogDto> MapAuctions(IEnumerable<Auction> auctions, string sellerLogin = null, string customerLogin = null)
        {
            return auctions.Select(auction => new AuctionCatalogDto
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

                SellerLogin = sellerLogin ?? _userRepository.GetUser(auction.SellerId).Login,
                CustomerLogin = customerLogin ?? ((auction.CustomerId == null) ? null : _userRepository.GetUser(auction.CustomerId.Value).Login),

                CategoryName = auction.Lot.Category.Name,
                LotName = auction.Lot.Name,
                PaintingDate = auction.Lot.PaintingDate,
                Photo = auction.Lot.Photo,
                Description = auction.Lot.Description
            });
        }
    }
}