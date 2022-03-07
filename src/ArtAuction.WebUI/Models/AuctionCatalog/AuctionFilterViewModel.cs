using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArtAuction.WebUI.Models.AuctionCatalog
{
    public class AuctionFilterViewModel
    {
        public IEnumerable<SelectListItem> Categories { get; set; }
        public decimal? MinCurrentPrice { get; set; }
        public decimal? MaxCurrentPrice { get; set; }
    }
}
