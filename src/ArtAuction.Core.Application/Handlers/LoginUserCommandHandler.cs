using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.DTO;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Application.Interfaces.Services;
using AutoMapper;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
        
        public LoginUserCommandHandler(IUserRepository userRepository, IPasswordService passwordService, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _mapper = mapper;
        }
        
        public async Task<UserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(request.Login);
            var passwordHash = _passwordService.GetHash(request.Password);
            
            if (user != null && passwordHash.Equals(user.Password))
            {
                return _mapper.Map<UserDto>(user);
            }
            
            return null;
        }
    }
}