using System;
using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class CreateVipCommand : IRequest
    {
        public Guid UserId { get; set; }
    }
}