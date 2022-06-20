using System.Collections.Generic;
using ArtAuction.Core.Application.DTO;
using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class GetUserAuctionsCommand : IRequest<IEnumerable<AuctionCatalogDto>>
    {
        public GetUserAuctionsCommand(string userLogin)
        {
            UserLogin = userLogin;
        }

        public string UserLogin { get; }
    }
}