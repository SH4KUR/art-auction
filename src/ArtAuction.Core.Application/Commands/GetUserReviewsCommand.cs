using System.Collections.Generic;
using ArtAuction.Core.Application.DTO;
using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class GetUserReviewsCommand : IRequest<IEnumerable<ReviewDto>>
    {
        public GetUserReviewsCommand(string userLogin)
        {
            UserLogin = userLogin;
        }

        public string UserLogin { get; set; }
    }
}