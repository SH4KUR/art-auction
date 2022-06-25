using System.Collections.Generic;
using ArtAuction.Core.Application.DTO;
using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class GetUserComplaintsCommand : IRequest<IEnumerable<ComplaintDto>>
    {
        public GetUserComplaintsCommand(string userLogin)
        {
            UserLogin = userLogin;
        }

        public string UserLogin { get; set; }
    }
}