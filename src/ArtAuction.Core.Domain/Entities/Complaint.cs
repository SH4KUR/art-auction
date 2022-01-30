using System;

namespace ArtAuction.Core.Domain.Entities
{
    public class Complaint
    {
        public Guid ComplaintId { get; set; }
        public Guid UserIdFrom { get; set; }
        public Guid UserIdOn { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public bool IsProcessed { get; set; }
    }
}