using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.WebUI.Models.AuctionCatalog;
using ArtAuction.WebUI.Models.Profile;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ArtAuction.WebUI.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ProfileController(IMediator mediator, IMapper mapper, IConfiguration configuration)
        {
            _mediator = mediator;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userLogin = User?.FindFirst(ClaimTypes.Name)?.Value;
            if (userLogin == null)
            {
                RedirectToAction("Index", "Home"); // TODO: Throw
            }

            var model = _mapper.Map<UserViewModel>(await _mediator.Send(new GetUserCommand(userLogin)));
            
            ViewData["AccountBalance"] = await _mediator.Send(new GetCurrentAccountBalanceCommand(userLogin));
            ViewData["VipStatusCost"] = Convert.ToDecimal(_configuration["App:VipStatusCost"]);
            ViewData["VipStatusDaysCount"] = Convert.ToInt32(_configuration["App:VipStatusDaysCount"]);

            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUserProfile(string userLogin)
        {
            var userAuctions = await _mediator.Send(new GetUserAuctionsCommand(userLogin));
            
            var model = new UserProfileViewModel
            {
                User = _mapper.Map<UserViewModel>(await _mediator.Send(new GetUserCommand(userLogin))),
                Auctions = userAuctions.Select(a => _mapper.Map<AuctionViewModel>(a)),
            };

            return View("UserProfile", model);
        }
    }
}