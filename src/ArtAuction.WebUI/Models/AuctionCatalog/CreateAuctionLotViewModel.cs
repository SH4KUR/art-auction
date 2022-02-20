using System;
using System.ComponentModel.DataAnnotations;

namespace ArtAuction.WebUI.Models.AuctionCatalog
{
    public class CreateAuctionLotViewModel
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Category { get; set; }
        
        [Required]
        public string PaintingDate { get; set; }
        
        [Required]
        public string Photo { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        public string SellerLogin { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartBillingDate { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndBillingDate { get; set; }
        
        [Required]
        [DataType(DataType.Currency)]
        public decimal StartPrice { get; set; }
        
        [DataType(DataType.Currency)]
        public decimal? FullPrice { get; set; }
        
        [Required]
        [DataType(DataType.Currency)]
        public decimal BidStep { get; set; }
    }
}