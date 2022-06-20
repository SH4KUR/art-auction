using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.Interfaces.Repositories;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class GetAuctionsToCloseCommandHandler : IRequestHandler<GetAuctionsToCloseCommand, IEnumerable<string>>
    {
        private readonly IAuctionRepository _auctionRepository;

        public GetAuctionsToCloseCommandHandler(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        public async Task<IEnumerable<string>> Handle(GetAuctionsToCloseCommand request, CancellationToken cancellationToken)
        {
            return await _auctionRepository.GetAuctionsToCloseAsync();
        }
    }
}