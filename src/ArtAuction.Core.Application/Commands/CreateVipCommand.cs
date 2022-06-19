using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class CreateVipCommand : IRequest
    {
        public string UserLogin { get; set; }
    }
}