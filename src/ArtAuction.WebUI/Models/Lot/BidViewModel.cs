using System;

namespace ArtAuction.WebUI.Models.Lot
{
    public class BidViewModel
    {
        public string UserLogin { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Sum { get; set; }
    }
}