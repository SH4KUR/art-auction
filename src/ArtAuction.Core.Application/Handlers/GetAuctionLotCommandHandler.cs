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
    public class GetAuctionLotCommandHandler : IRequestHandler<GetAuctionLotCommand, AuctionLotDto>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IUserRepository _userRepository;

        public GetAuctionLotCommandHandler(IAuctionRepository auctionRepository, IUserRepository userRepository)
        {
            _auctionRepository = auctionRepository;
            _userRepository = userRepository;
        }

        public async Task<AuctionLotDto> Handle(GetAuctionLotCommand request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetAuctionAsync(request.AuctionNumber);
            
            return new AuctionLotDto
            {
                AuctionLot = MapAuctionCatalog(auction),
                Messages = MapMessages(auction.Messages),
                Bids = MapBids(auction.Bids)
            };
        }

        private AuctionCatalogDto MapAuctionCatalog(Auction auction)
        {
            return new()
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

        private IEnumerable<MessageDto> MapMessages(IEnumerable<Message> messages)
        {
            return messages.Select(message => new MessageDto
            {
                UserLogin = _userRepository.GetUser(message.UserId).Login,
                DateTime = message.DateTime,
                MessageText = message.MessageText,
                IsAdmin = message.IsAdmin
            });
        }

        private IEnumerable<BidDto> MapBids(IEnumerable<Bid> bids)
        {
            return bids.Select(bid => new BidDto
            {
                UserLogin = _userRepository.GetUser(bid.UserId).Login,
                DateTime = bid.DateTime,
                Sum = bid.Sum
            });
        }
    }
}