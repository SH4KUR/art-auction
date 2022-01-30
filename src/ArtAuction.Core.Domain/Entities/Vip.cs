using System;

namespace ArtAuction.Core.Domain.Entities
{
    public class Vip
    {
        public Guid VipId { get; set; }
        public Guid OperationId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateUntil { get; set; }
    }
}