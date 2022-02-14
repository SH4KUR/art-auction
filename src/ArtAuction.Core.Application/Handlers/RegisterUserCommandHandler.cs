using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.DTO;
using ArtAuction.Core.Application.Exceptions;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Application.Interfaces.Services;
using ArtAuction.Core.Domain.Entities;
using AutoMapper;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordService passwordService, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var isUserExist = await _userRepository.IsUserAlreadyRegisteredAsync(request.Login, request.Email);
            if (isUserExist)
            {
                throw new UserAlreadyRegisteredException("User with such Login or Email is already registered!");
            }
            
            var addedUser = _mapper.Map<User>(request);
            addedUser.Password = _passwordService.GetHash(request.Password);

            await _userRepository.AddUserAsync(addedUser);

            var registeredUser = await _userRepository.GetUserAsync(addedUser.Login);
            return _mapper.Map<UserDto>(registeredUser);
        }
    }
}