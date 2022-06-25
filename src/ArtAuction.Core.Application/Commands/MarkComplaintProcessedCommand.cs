using System;
using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class MarkComplaintProcessedCommand : IRequest
    {
        public MarkComplaintProcessedCommand(Guid complaintId)
        {
            ComplaintId = complaintId;
        }

        public Guid ComplaintId { get; }
    }
}