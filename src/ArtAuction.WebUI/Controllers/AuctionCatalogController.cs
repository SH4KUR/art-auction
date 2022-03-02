using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Domain.Enums;
using ArtAuction.WebUI.Models.AuctionCatalog;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArtAuction.WebUI.Controllers
{
    public class AuctionCatalogController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuctionCatalogController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await GetCategorySelectListItems();
            return View();
        }

        private async Task<IEnumerable<SelectListItem>> GetCategorySelectListItems()
        {
            var categories = await _mediator.Send(new GetCategoriesCommand());
            return categories.Select(category => new SelectListItem { Text = category, Value = category });
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
                var isCreated = await _mediator.Send(_mapper.Map<CreateAuctionCommand>(model));
                if (isCreated)
                {
                    return RedirectToAction("Index", "Home");   // TODO: Redirect to Profile/MyAuctions
                }
                
                ModelState.AddModelError(string.Empty, "Something went wrong!");   // TODO: Add correct errors handling
            }
            
            ViewBag.Categories = new SelectList(await _mediator.Send(new GetCategoriesCommand()));
            return View(model);
        }
    }
}