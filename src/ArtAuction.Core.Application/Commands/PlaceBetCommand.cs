using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class PlaceBetCommand : IRequest
    {
        public string UserLogin { get; set; }
        public int AuctionNumber { get; set; }
        public decimal Sum { get; set; }
    }
}