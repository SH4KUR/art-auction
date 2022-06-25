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
    public class GetComplaintsCommandHandler : IRequestHandler<GetComplaintsCommand, IEnumerable<ComplaintDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetComplaintsCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ComplaintDto>> Handle(GetComplaintsCommand request, CancellationToken cancellationToken)
        {
            var complaints = await _userRepository.GetComplaints();
            return complaints.Select(complaint => new ComplaintDto
            {
                UserLoginOn = _userRepository.GetUser(complaint.UserIdOn).Login,
                UserLoginFrom = _userRepository.GetUser(complaint.UserIdFrom).Login,
                DateTime = complaint.DateTime,
                Description = complaint.Description,
                IsProcessed = complaint.IsProcessed
            });
        }
    }
}