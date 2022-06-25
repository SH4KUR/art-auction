using System;

namespace ArtAuction.Core.Application.DTO
{
    public class ReviewDto
    {
        public string UserLoginFrom { get; set; }
        public string UserLoginOn { get; set; }
        public DateTime DateTime { get; set; }
        public int Rate { get; set; }
        public string Description { get; set; }
    }
}