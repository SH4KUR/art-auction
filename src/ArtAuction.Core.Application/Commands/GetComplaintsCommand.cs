using System.Collections.Generic;
using ArtAuction.Core.Application.DTO;
using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class GetComplaintsCommand : IRequest<IEnumerable<ComplaintDto>>
    {
    }
}