using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class AddAuctionMessageCommand : IRequest
    {
        public int AuctionNumber { get; set; }
        public string Login { get; set; }
        public string Message { get; set; }
    }
}