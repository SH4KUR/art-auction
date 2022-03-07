using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class GetCurrentAccountBalanceCommandHandler : IRequestHandler<GetCurrentAccountBalanceCommand, decimal>
    {
        public Task<decimal> Handle(GetCurrentAccountBalanceCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}