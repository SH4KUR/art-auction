using System;

namespace ArtAuction.Core.Domain.Entities
{
    public class Lot
    {
        public Guid LotId { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }
        public string PaintingDate { get; set; }
        public byte[] Photo { get; set; }
        public string Description { get; set; }
    }
}