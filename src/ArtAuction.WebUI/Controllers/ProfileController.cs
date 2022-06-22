using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.WebUI.Models.Profile;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;

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
                RedirectToAction("Index", "Home"); // TODO: Throw
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
            await _mediator.Send(new BuyVipCommand {UserLogin = User?.FindFirst(ClaimTypes.Name)?.Value});
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ReplenishPersonalAccount(decimal replenishmentAmount)
        {
            ViewData["StripePublicKey"] = _configuration["StripeAPI:PublicKey"];

            return View("ReplenishPersonalAccount", replenishmentAmount);
        }

        [HttpPost("CreatePaymentIntent")]
        public JsonResult CreatePaymentIntent([FromBody] PaymentIntentCreateRequest model)
        {
            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
            {
                Amount = (long?)model.ReplenishmentAmount,
                Currency = "usd",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                }
            });

            return Json(new { clientSecret = paymentIntent.ClientSecret });
        }
        
        public IActionResult BuyVipByCard()
        {
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new()
                    {
                        Price = _configuration["StripeAPI:VipPriceId"],
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = "https://localhost:44302/Profile",
                CancelUrl = "https://localhost:44302/Profile",
            };
            
            var session = new SessionService().Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
    }

    public class PaymentIntentCreateRequest
    {
        [JsonProperty("replenishmentAmount")]
        public decimal ReplenishmentAmount { get; set; }
    }
}