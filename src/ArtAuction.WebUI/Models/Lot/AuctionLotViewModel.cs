using System.Collections.Generic;
using ArtAuction.WebUI.Models.AuctionCatalog;

namespace ArtAuction.WebUI.Models.Lot
{
    public class AuctionLotViewModel
    {
        public AuctionViewModel AuctionLot { get; set; }
        public IEnumerable<MessageViewModel> Messages { get; set; }
        public IEnumerable<BidViewModel> Bids { get; set; }
    }
}