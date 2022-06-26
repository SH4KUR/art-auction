using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class UnblockUserCommand : IRequest
    {
        public UnblockUserCommand(string userLogin)
        {
            UserLogin = userLogin;
        }

        public string UserLogin { get; }
    }
}