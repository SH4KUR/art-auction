using ArtAuction.Core.Application.DTO;
using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class GetAuctionLotCommand : IRequest<AuctionLotDto>
    {
        public GetAuctionLotCommand(int auctionNumber)
        {
            AuctionNumber = auctionNumber;
        }

        public int AuctionNumber { get; }
    }
}