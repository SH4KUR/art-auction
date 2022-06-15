using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class GetCurrentAccountBalanceCommand : IRequest<decimal>
    {
        public GetCurrentAccountBalanceCommand(string userLogin)
        {
            UserLogin = userLogin;
        }

        public string UserLogin { get; }
    }
}