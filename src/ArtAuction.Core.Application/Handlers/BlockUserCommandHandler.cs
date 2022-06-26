using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.Interfaces.Repositories;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAdminRepository _adminRepository;

        public BlockUserCommandHandler(IUserRepository userRepository, IAdminRepository adminRepository)
        {
            _userRepository = userRepository;
            _adminRepository = adminRepository;
        }

        public async Task<Unit> Handle(BlockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(request.UserLogin);
            await _adminRepository.BlockUser(user.UserId);
            
            return Unit.Value;
        }
    }
}
