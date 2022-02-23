using System;
using MediatR;

namespace ArtAuction.Core.Application.Commands
{
    public class CreateAuctionCommand : IRequest<bool>
    {
        public string LotName { get; set; }
        public string CategoryName { get; set; }
        public string PaintingDate { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public string SellerLogin { get; set; }
        public DateTime StartBillingDate { get; set; }
        public DateTime EndBillingDate { get; set; }
        public decimal StartPrice { get; set; }
        public decimal? FullPrice { get; set; }
        public decimal BidStep { get; set; }
    }
}