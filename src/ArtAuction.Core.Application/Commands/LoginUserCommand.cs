using ArtAuction.Core.Application.DTO;
using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class LoginUserCommand : IRequest<UserDto>
    {
        public LoginUserCommand(string login, string password)
        {
            Login = login;
            Password = password;
        }
        
        public string Login { get; }
        public string Password { get; }
    }
}