using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class BuyVipCommand : IRequest
    {
        public string UserLogin { get; set; }
        public bool ByCard { get; set; }
    }
}