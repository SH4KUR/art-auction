using System;

namespace ArtAuction.Core.Application.DTO
{
    public class ComplaintDto
    {
        public string UserIdFrom { get; set; }
        public string UserIdOn { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public bool IsProcessed { get; set; }
    }
}