using ArtAuction.Core.Application.DTO;
using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class GetAuctionLotCommand : IRequest<AuctionLotDto>
    {
        public int AuctionNumber { get; set; }
    }
}