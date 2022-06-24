﻿using System;

namespace ArtAuction.WebUI.Models.AuctionCatalog
{
    public class AuctionViewModel
    {
        public int AuctionNumber { get; set; }
        public bool IsClosed { get; set; }
        public bool IsVip { get; set; }

        public DateTime CreationDateTime { get; set; }
        public DateTime StartBillingDateTime { get; set; }
        public DateTime EndBillingDateTime { get; set; }

        public decimal StartPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal? FullPrice { get; set; }
        public decimal BidStep { get; set; }
        public int BidsCount { get; set; }

        public string SellerLogin { get; set; }
        public string CustomerLogin { get; set; }

        public string CategoryName { get; set; }
        public string LotName { get; set; }
        public string Description { get; set; }
        public string PaintingDate { get; set; }
        
        public byte[] Image { get; set; }
    }
}