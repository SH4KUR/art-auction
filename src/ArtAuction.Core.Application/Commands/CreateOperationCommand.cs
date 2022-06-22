using System;
using ArtAuction.Core.Domain.Enums;
using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class CreateOperationCommand : IRequest
    {
        public string UserLogin { get; set; }
        public OperationType OperationType { get; set; }
        public decimal Sum { get; set; }
        public string Description { get; set; }
    }
}