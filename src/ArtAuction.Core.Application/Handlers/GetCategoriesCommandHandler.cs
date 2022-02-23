using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.Interfaces.Repositories;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class GetCategoriesCommandHandler : IRequestHandler<GetCategoriesCommand, IEnumerable<string>>
    {
        private readonly IAuctionRepository _auctionRepository;

        public GetCategoriesCommandHandler(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        public async Task<IEnumerable<string>> Handle(GetCategoriesCommand request, CancellationToken cancellationToken)
        {
            var categories = await _auctionRepository.GetCategories();
            return categories.Select(c => c.Name);
        }
    }
}