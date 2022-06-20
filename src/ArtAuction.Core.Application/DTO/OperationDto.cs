using System;
using ArtAuction.Core.Domain.Enums;

namespace ArtAuction.Core.Application.DTO
{
    public class OperationDto
    {
        public DateTime DateTime { get; set; }
        public OperationType OperationType { get; set; }
        public decimal SumBefore { get; set; }
        public decimal SumOperation { get; set; }
        public decimal SumAfter { get; set; }
        public string Description { get; set; }
    }
}