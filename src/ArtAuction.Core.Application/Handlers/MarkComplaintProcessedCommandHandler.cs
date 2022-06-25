using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.Interfaces.Repositories;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class MarkComplaintProcessedCommandHandler : IRequestHandler<MarkComplaintProcessedCommand, Unit>
    {
        private readonly IAdminRepository _adminRepository;

        public MarkComplaintProcessedCommandHandler(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Unit> Handle(MarkComplaintProcessedCommand request, CancellationToken cancellationToken)
        {
            await _adminRepository.MarkComplaintProcessed(request.ComplaintId);
            return Unit.Value;
        }
    }
}