using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class BuyFullPriceLotCommand : IRequest
    {
        public string UserLogin { get; set; }
        public int AuctionNumber { get; set; }
    }
}