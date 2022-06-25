using System.Collections.Generic;
using ArtAuction.WebUI.Models.AuctionCatalog;

namespace ArtAuction.WebUI.Models.Profile
{
    public class UserProfileViewModel
    {
        public UserViewModel User { get; set; }
        public IEnumerable<AuctionViewModel> Auctions { get; set; }
        public IEnumerable<ComplaintViewModel> Complaints { get; set; }
        public IEnumerable<ReviewViewModel> Reviews { get; set; }
    }
}