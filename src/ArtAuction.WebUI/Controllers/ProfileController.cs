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
            var userOperations = await _mediator.Send(new GetAccountOperationsCommand(userLogin));

            var model = new UserOperationsViewModel
            {
                User = _mapper.Map<UserViewModel>(await _mediator.Send(new GetUserCommand(userLogin))),
                Operations = userOperations.Select(o => _mapper.Map<OperationViewModel>(o))
            };

            ViewData["AccountBalance"] = await _mediator.Send(new GetCurrentAccountBalanceCommand(userLogin));
            ViewData["VipStatusCost"] = Convert.ToDecimal(_configuration["App:VipStatusCost"]);
            ViewData["VipStatusDaysCount"] = Convert.ToInt32(_configuration["App:VipStatusDaysCount"]);

            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUserProfile(string userLogin)
        {
            var auctions = await _mediator.Send(new GetUserAuctionsCommand(userLogin));
            var reviews = await _mediator.Send(new GetUserReviewsCommand(userLogin));
            var complaints = await _mediator.Send(new GetUserComplaintsCommand(userLogin));

            var model = new UserProfileViewModel
            {
                User = _mapper.Map<UserViewModel>(await _mediator.Send(new GetUserCommand(userLogin))),
                Auctions = auctions.Select(a => _mapper.Map<AuctionViewModel>(a)),
                Reviews = reviews.Select(r => _mapper.Map<ReviewViewModel>(r)),
                Complaints = complaints.Select(c => _mapper.Map<ComplaintViewModel>(c))
            };

            return View("UserProfile", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddComplaint(ComplaintViewModel model)
        {
            await _mediator.Send(new AddUserComplaintCommand
            {
                UserLoginOn = model.UserLoginOn,
                UserLoginFrom = User?.FindFirst(ClaimTypes.Name)?.Value,
                Description = model.Description
            });
            
            return RedirectToAction("GetUserProfile", new { userLogin = model.UserLoginOn });
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(ReviewViewModel model)
        {
            await _mediator.Send(new AddUserReviewCommand
            {
                UserLoginOn = model.UserLoginOn,
                UserLoginFrom = User?.FindFirst(ClaimTypes.Name)?.Value,
                Rate = model.Rate,
                Description = model.Description
            });

            return RedirectToAction("GetUserProfile", new { userLogin = model.UserLoginOn });
        }
    }
}