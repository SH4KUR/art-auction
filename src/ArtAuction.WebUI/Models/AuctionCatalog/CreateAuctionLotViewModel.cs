using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ArtAuction.WebUI.Models.AuctionCatalog
{
    public class CreateAuctionLotViewModel
    {
        [Required]
        public string LotName { get; set; }
        
        [Required]
        public string CategoryName { get; set; }
        
        [Required]
        public string PaintingDate { get; set; }
        
        [Required]
        public IFormFile Image { get; set; }
        
        [Required]
        public string Description { get; set; }

        [Required]
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