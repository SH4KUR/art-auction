using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class CloseAuctionCommand : IRequest
    {
        public CloseAuctionCommand(int auctionNumber)
        {
            AuctionNumber = auctionNumber;
        }

        public int AuctionNumber { get; }
    }
}