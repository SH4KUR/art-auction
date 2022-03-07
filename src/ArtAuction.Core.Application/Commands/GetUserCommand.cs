using ArtAuction.Core.Application.DTO;
using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class GetUserCommand : IRequest<UserDto>
    {
        public GetUserCommand(string userLogin)
        {
            UserLogin = userLogin;
        }

        public string UserLogin { get; }
    }
}