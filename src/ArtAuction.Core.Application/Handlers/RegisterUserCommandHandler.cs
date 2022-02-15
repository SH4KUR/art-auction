using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.Exceptions;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Application.Interfaces.Services;
using ArtAuction.Core.Domain.Entities;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var isUserExist = await _userRepository.IsUserAlreadyRegisteredAsync(request.Login, request.Email);
            if (isUserExist)
            {
                throw new UserAlreadyRegisteredException("User with such Login or Email is already registered!");
            }

            var addedUser = new User
            {
                Login = request.Login,
                Email = request.Email,
                Password = _passwordService.GetHash(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Patronymic = request.Patronymic,
                Address = request.Address,
                BirthDate = request.BirthDate,
                Role = request.Role,
                IsBlocked = false,
                IsVip = false
            };
            
            await _userRepository.AddUserAsync(addedUser);
            
            return Unit.Value;
        }
    }
}