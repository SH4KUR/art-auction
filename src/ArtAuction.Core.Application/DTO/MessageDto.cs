using System;

namespace ArtAuction.Core.Application.DTO
{
    public class MessageDto
    {
        public string UserLogin { get; set; }
        public DateTime DateTime { get; set; }
        public string MessageText { get; set; }
        public bool IsAdmin { get; set; }
    }
}