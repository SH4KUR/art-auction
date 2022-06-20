using System.Collections.Generic;
using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class GetAuctionsToCloseCommand : IRequest<IEnumerable<string>>
    {
    }
}