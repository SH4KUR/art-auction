using System;

namespace ArtAuction.WebUI.Models.Profile
{
    public class ReviewViewModel
    {
        public string UserLoginFrom { get; set; }
        public string UserLoginOn { get; set; }
        public DateTime DateTime { get; set; }
        public int Rate { get; set; }
        public string Description { get; set; }
    }
}