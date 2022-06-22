using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.WebUI.Models.Profile;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ArtAuction.WebUI.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Index()
        {
            var userLogin = User?.FindFirst(ClaimTypes.Name)?.Value;
            if (userLogin == null)
            {
                RedirectToAction("Index", "Home");  // TODO: Throw
            }

            var model = _mapper.Map<UserProfileViewModel>(await _mediator.Send(new GetUserCommand(userLogin)));
            model.AccountBalance = await _mediator.Send(new GetCurrentAccountBalanceCommand(userLogin));

            ViewData["VipStatusCost"] = Convert.ToDecimal(_configuration["App:VipStatusCost"]);
            ViewData["VipStatusDaysCount"] = Convert.ToInt32(_configuration["App:VipStatusDaysCount"]);

            return View(model);
        }
        
        public async Task<IActionResult> BuyVipStatus()
        {
            // TODO: Redirect if there isn't enough money to buy a VIP
            await _mediator.Send(new BuyVipCommand { UserLogin = User?.FindFirst(ClaimTypes.Name)?.Value });
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ReplenishPersonalAccount(decimal replenishmentAmount)
        {
            
            return RedirectToAction("Index");
        }
    }
}