using System;

namespace ArtAuction.WebUI.Models
{
    public class PageViewModel
    {
        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; }
        
        public bool HasPreviousPage => (PageNumber > 1);
        public bool HasNextPage => (PageNumber < TotalPages);
    }
}