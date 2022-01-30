using System;

namespace ArtAuction.Core.Domain.Entities
{
    public class Operation
    {
        public Guid OperationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime DateTime { get; set; }
        public decimal SumBefore { get; set; }
        public decimal SumOperation { get; set; }
        public decimal SumAfter { get; set; }
        public string Description { get; set; }
    }
}