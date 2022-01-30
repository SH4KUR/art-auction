using System;

namespace ArtAuction.Core.Domain.Entities
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
        public decimal Sum { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}