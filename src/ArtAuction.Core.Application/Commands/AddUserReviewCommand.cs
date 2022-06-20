using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class AddUserReviewCommand : IRequest
    {
        public string UserLoginOn { get; set; }
        public string UserLoginFrom { get; set; }
        public int Rate { get; set; }
        public string Description { get; set; }
    }
}