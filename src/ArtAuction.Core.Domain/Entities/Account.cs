using System;
using System.Collections.Generic;

namespace ArtAuction.Core.Domain.Entities
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
        public decimal Sum { get; set; }
        public DateTime LastUpdate { get; set; }

        public IEnumerable<Vip> Vips { get; set; }
        public IEnumerable<Operation> Operations { get; set; }
    }
}