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
    public class GetUserComplaintsCommandHandler : IRequestHandler<GetUserComplaintsCommand, IEnumerable<ComplaintDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserComplaintsCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ComplaintDto>> Handle(GetUserComplaintsCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(request.UserLogin);
            
            var complaints = await _userRepository.GetUserComplaints(user.UserId);
            return complaints.Select(complaint => new ComplaintDto
            {
                ComplaintId = complaint.ComplaintId,
                UserLoginOn = user.Login,
                UserLoginFrom = _userRepository.GetUser(complaint.UserIdFrom).Login,
                DateTime = complaint.DateTime,
                Description = complaint.Description,
                IsProcessed = complaint.IsProcessed
            });
        }
    }
}