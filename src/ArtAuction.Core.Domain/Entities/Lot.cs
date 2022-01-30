using System;

namespace ArtAuction.Core.Domain.Entities
{
    public class Lot
    {
        public Guid LotId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string PaintingDate { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
    }
}