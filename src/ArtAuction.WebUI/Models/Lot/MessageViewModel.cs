using System;

namespace ArtAuction.WebUI.Models.Lot
{
    public class MessageViewModel
    {
        public string UserLogin { get; set; }
        public DateTime DateTime { get; set; }
        public string MessageText { get; set; }
        public bool IsAdmin { get; set; }
    }
}