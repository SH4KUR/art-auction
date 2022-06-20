using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Domain.Entities;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class AddUserComplaintCommandHandler : IRequestHandler<AddUserComplaintCommand, Unit>
    {
        private readonly IUserRepository _userRepository;

        public AddUserComplaintCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(AddUserComplaintCommand request, CancellationToken cancellationToken)
        {
            var userOn = await _userRepository.GetUserAsync(request.UserLoginOn);
            var userFrom = await _userRepository.GetUserAsync(request.UserLoginFrom);

            await _userRepository.AddComplaintAsync(new Complaint
            {
                UserIdFrom = userFrom.UserId,
                UserIdOn = userOn.UserId,
                Description = request.Description
            });

            return Unit.Value;
        }
    }
}