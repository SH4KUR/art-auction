using System;

namespace ArtAuction.Core.Application.DTO
{
    public class BidDto
    {
        public string UserLogin { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Sum { get; set; }
    }
}