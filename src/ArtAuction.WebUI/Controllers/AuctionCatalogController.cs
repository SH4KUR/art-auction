using System;
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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArtAuction.WebUI.Controllers
{
    public class AuctionCatalogController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        private const int LotsOnPage = 10;
        
        public AuctionCatalogController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string[] category,
            Sort sort,
            decimal? minCurrentPrice,
            decimal? maxCurrentPrice,
            int pageNumber = 1)
        {
            category = category.Length == 1 ? category.First().Split(',') : category;   // workaround

            var auctionCatalogWithPagingDto = await _mediator.Send(new GetAuctionCatalogCommand(
                (SortingRule) sort,
                category,
                minCurrentPrice, 
                maxCurrentPrice, 
                pageNumber, 
                LotsOnPage, 
                false)
            );

            var model = new AuctionCatalogViewModel
            {
                Auctions = auctionCatalogWithPagingDto.Auctions.Select(a => _mapper.Map<AuctionViewModel>(a)),
                Sort = sort,
                Filter =
                {
                    Categories = await GetCategorySelectListItems(category),
                    MinCurrentPrice = minCurrentPrice,
                    MaxCurrentPrice = maxCurrentPrice
                },
                Pagination =
                {
                    TotalPages = (int) Math.Ceiling(auctionCatalogWithPagingDto.Auctions.Count() / (double) LotsOnPage),
                    PageNumber = pageNumber
                }
            };
            
            return View(model);
        }

        private async Task<IEnumerable<SelectListItem>> GetCategorySelectListItems(string[] selectedCategories = null)
        {
            var categories = await _mediator.Send(new GetCategoriesCommand());
            var selectListItems = categories.Select(category => new SelectListItem { Text = category, Value = category }).ToArray();

            if (selectedCategories != null && selectedCategories.Any())
            {
                foreach (var selectListItem in selectListItems)
                {
                    if (selectedCategories.Any(c => c == selectListItem.Value))
                    {
                        selectListItem.Selected = true;
                    }
                }
            }

            return selectListItems;
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