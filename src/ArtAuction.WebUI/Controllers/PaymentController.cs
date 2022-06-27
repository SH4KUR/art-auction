using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Domain.Enums;
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
    public class PaymentController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public PaymentController(IMediator mediator, IMapper mapper, IConfiguration configuration)
        {
            _mediator = mediator;
            _mapper = mapper;
            _configuration = configuration;
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

        [HttpPost]
        public IActionResult ReplenishPersonalAccount(decimal replenishmentAmount)
        {
            ViewData["StripePublicKey"] = _configuration["StripeAPI:PublicKey"];

            return View("ReplenishPersonalAccount", replenishmentAmount);
        }

        [Route("[controller]/ReplenishmentConfirm/{sum}")]
        public async Task<IActionResult> ReplenishmentConfirm(decimal sum)
        {
            await _mediator.Send(new CreateOperationCommand
            {
                UserLogin = User?.FindFirst(ClaimTypes.Name)?.Value,
                OperationType = OperationType.Replenishment,
                Sum = sum,
                Description = $"Account Replenishment: {sum}"
            });

            return RedirectToAction("ReplenishmentSuccessful");
        }
        
        public IActionResult ReplenishmentSuccessful()
        {
            return View();
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
                SuccessUrl = "https://localhost:44302/Payment/BuyVipStatusByCard",
                CancelUrl = "https://localhost:44302/Profile",
            };
            
            var session = new SessionService().Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        [Route("[controller]/BuyVipStatus")]
        public async Task<IActionResult> BuyVipStatus()
        {
            await _mediator.Send(new BuyVipCommand { UserLogin = User?.FindFirst(ClaimTypes.Name)?.Value });
            return RedirectToAction("BuyVipSuccessful");
        }

        [Route("[controller]/BuyVipStatusByCard")]
        public async Task<IActionResult> BuyVipStatusByCard()
        {
            await _mediator.Send(new BuyVipCommand { UserLogin = User?.FindFirst(ClaimTypes.Name)?.Value, ByCard = true });
            return RedirectToAction("BuyVipSuccessful");
        }

        public IActionResult BuyVipSuccessful()
        {
            return View();
        }
    }

    public class PaymentIntentCreateRequest
    {
        [JsonProperty("replenishmentAmount")]
        public decimal ReplenishmentAmount { get; set; }
    }
}