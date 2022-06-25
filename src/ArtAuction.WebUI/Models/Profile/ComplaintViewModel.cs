using System;

namespace ArtAuction.WebUI.Models.Profile
{
    public class ComplaintViewModel
    {
        public string UserLoginFrom { get; set; }
        public string UserLoginOn { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public bool IsProcessed { get; set; }
    }
}