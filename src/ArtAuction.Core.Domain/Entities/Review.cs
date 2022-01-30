using System;

namespace ArtAuction.Core.Domain.Entities
{
    public class Review
    {
        public Guid ReviewId { get; set; }
        public Guid UserIdFrom { get; set; }
        public Guid UserIdOn { get; set; }
        public DateTime DateTime { get; set; }
        public int Rate { get; set; }
        public string Description { get; set; }
    }
}