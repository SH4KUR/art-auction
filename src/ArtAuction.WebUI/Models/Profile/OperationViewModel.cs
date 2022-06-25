using System;
using ArtAuction.Core.Domain.Enums;

namespace ArtAuction.WebUI.Models.Profile
{
    public class OperationViewModel
    {
        public DateTime DateTime { get; set; }
        public OperationType OperationType { get; set; }
        public decimal SumBefore { get; set; }
        public decimal SumOperation { get; set; }
        public decimal SumAfter { get; set; }
        public string Description { get; set; }
    }
}