using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class BlockUserCommand : IRequest
    {
        public BlockUserCommand(string userLogin)
        {
            UserLogin = userLogin;
        }

        public string UserLogin { get; }
    }
}