using System;

namespace ArtAuction.Core.Application.DTO
{
    public class ComplaintDto
    {
        public Guid ComplaintId { get; set; }
        public string UserLoginFrom { get; set; }
        public string UserLoginOn { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public bool IsProcessed { get; set; }
    }
}