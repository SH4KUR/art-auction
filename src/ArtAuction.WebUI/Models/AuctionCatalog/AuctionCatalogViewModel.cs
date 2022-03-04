using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArtAuction.WebUI.Models.AuctionCatalog
{
    public class AuctionCatalogViewModel
    {
        public IEnumerable<AuctionViewModel> Auctions { get; set; }
        public Sort Sort { get; set; }
        public AuctionFilterViewModel Filter { get; set; } = new();
        public PageViewModel Pagination { get; set; } = new();
    }

    public enum Sort
    {
        [Display(Name = "Default")] Default,
        [Display(Name = "Current price (from min to max)")] CurrentPriceAsc,
        [Display(Name = "Current price (from max to min)")] CurrentPriceDesc,
        [Display(Name = "Creating Auction (from new to old)")] DateTimeAuctionCreateAsc,
        [Display(Name = "Creating Auction (from old to new)")] DateTimeAuctionCreateDesc,
        [Display(Name = "End Auction (from short to long)")] DateTimeAuctionEndAsc,     // TODO: correct display naming
        [Display(Name = "End Auction (from long to short)")] DateTimeAuctionEndDesc
    }
}