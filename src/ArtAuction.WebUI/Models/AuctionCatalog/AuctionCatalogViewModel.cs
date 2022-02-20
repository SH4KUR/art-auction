using System.Collections.Generic;

namespace ArtAuction.WebUI.Models.AuctionCatalog
{
    public class AuctionCatalogViewModel
    {
        public IEnumerable<AuctionViewModel> Auctions { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}