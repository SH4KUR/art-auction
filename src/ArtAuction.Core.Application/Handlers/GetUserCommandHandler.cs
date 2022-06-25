using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.DTO;
using ArtAuction.Core.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class GetUserCommandHandler : IRequestHandler<GetUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(request.UserLogin);
            var userDto = _mapper.Map<UserDto>(user);

            var reviews = (await _userRepository.GetUserReviews(user.UserId)).ToArray();
            userDto.AvgRate = reviews.Any() ? (decimal) reviews.Average(r => r.Rate) : decimal.Zero;

            return userDto;
        }
    }
}