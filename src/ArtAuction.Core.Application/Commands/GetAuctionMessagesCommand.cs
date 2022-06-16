using System.Collections.Generic;
using ArtAuction.Core.Application.DTO;
using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class GetAuctionMessagesCommand : IRequest<IEnumerable<MessageDto>>
    {
        public int AuctionNumber { get; set; }
    }
}