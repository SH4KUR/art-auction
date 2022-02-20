using ArtAuction.Core.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.WebUI.Controllers
{
    public class AuctionCatalogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = nameof(UserRole.Seller))]
        public IActionResult AddAuction()
        {
            return View();
        }
    }
}
