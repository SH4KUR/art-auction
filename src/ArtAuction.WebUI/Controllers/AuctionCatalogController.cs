using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Domain.Enums;
using ArtAuction.WebUI.Models.AuctionCatalog;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArtAuction.WebUI.Controllers
{
    public class AuctionCatalogController : Controller
    {
        private readonly IMediator _mediator;

        public AuctionCatalogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Seller))]
        public async Task<IActionResult> AddAuction()
        {
            ViewBag.Categories = new SelectList(await _mediator.Send(new GetCategoriesCommand()));
            return View();
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Seller))]
        public async Task<IActionResult> AddAuction(CreateAuctionLotViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Some code
                
                return RedirectToAction("Index", "Home");   // TODO: Redirect to Profile/MyAuctions
            }
            
            ViewBag.Categories = new SelectList(await _mediator.Send(new GetCategoriesCommand()));
            return View(model);
        }
    }
}