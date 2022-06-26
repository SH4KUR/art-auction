using System.Collections.Generic;
using ArtAuction.Core.Application.DTO;
using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class GetLastAuctionsCommand : IRequest<IEnumerable<AuctionCatalogDto>>
    {
    }
}