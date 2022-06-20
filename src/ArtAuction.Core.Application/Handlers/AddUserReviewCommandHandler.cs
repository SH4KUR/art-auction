using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Domain.Entities;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class AddUserReviewCommandHandler : IRequestHandler<AddUserReviewCommand, Unit>
    {
        private readonly IUserRepository _userRepository;

        public AddUserReviewCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(AddUserReviewCommand request, CancellationToken cancellationToken)
        {
            var userOn = await _userRepository.GetUserAsync(request.UserLoginOn);
            var userFrom = await _userRepository.GetUserAsync(request.UserLoginFrom);

            await _userRepository.AddReviewAsync(new Review
            {
                UserIdFrom = userFrom.UserId,
                UserIdOn = userOn.UserId,
                Rate = request.Rate,
                Description = request.Description
            });

            return Unit.Value;
        }
    }
}