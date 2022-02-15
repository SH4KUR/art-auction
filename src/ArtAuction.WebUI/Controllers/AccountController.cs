using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.WebUI.Models.Account;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        
        public AccountController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AccountLoginViewModel model)
        {
            try
            {
                return RedirectToAction(nameof(Login));
            }
            catch
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(AccountRegistrationViewModel model)
        {
            // TODO: Add UserAlreadyRegisteredException handling
            if (ModelState.IsValid)
            {
                await _mediator.Send(_mapper.Map<RegisterUserCommand>(model));
                return RedirectToAction("Index", "Home");   // TODO: Add user notification about registration
            }

            return View(model);
        }
    }
}