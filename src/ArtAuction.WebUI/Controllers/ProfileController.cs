using System.Security.Claims;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.WebUI.Models.Profile;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.WebUI.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        
        public ProfileController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
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
            
            return View(model);
        }
    }
}