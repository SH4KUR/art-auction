using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class AddUserComplaintCommand : IRequest
    {
        public string UserLoginOn { get; set; }
        public string UserLoginFrom { get; set; }
        public string Description { get; set; }
    }
}