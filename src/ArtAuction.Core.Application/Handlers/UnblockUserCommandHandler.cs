using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.Interfaces.Repositories;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class UnblockUserCommandHandler : IRequestHandler<UnblockUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAdminRepository _adminRepository;

        public UnblockUserCommandHandler(IUserRepository userRepository, IAdminRepository adminRepository)
        {
            _userRepository = userRepository;
            _adminRepository = adminRepository;
        }

        public async Task<Unit> Handle(UnblockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(request.UserLogin);
            await _adminRepository.UnblockUser(user.UserId);
            
            return Unit.Value;
        }
    }
}
