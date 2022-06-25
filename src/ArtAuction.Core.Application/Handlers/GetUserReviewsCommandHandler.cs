using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.DTO;
using ArtAuction.Core.Application.Interfaces.Repositories;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class GetUserReviewsCommandHandler : IRequestHandler<GetUserReviewsCommand, IEnumerable<ReviewDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserReviewsCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ReviewDto>> Handle(GetUserReviewsCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(request.UserLogin);

            var reviews = await _userRepository.GetUserReviews(user.UserId);
            return reviews.Select(review => new ReviewDto
            {
                UserIdOn = user.Login,
                UserIdFrom = _userRepository.GetUser(review.UserIdFrom).Login,
                DateTime = review.DateTime,
                Rate = review.Rate,
                Description = review.Description,
            });
        }
    }
}