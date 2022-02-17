using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.WebUI.Models.Account;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        public async Task<IActionResult> Login(AccountLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loggedUser = await _mediator.Send(new LoginUserCommand(model.Login, model.Password));
                if (loggedUser != null)
                {
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Role, loggedUser.Role.ToString()),
                        new(ClaimTypes.Name, loggedUser.Login)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.IsRemember
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login or password!");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            return RedirectToAction("Index", "Home");
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
            if (ModelState.IsValid)
            {
                var isRegistered = await _mediator.Send(_mapper.Map<RegisterUserCommand>(model));
                if (isRegistered)
                {
                    return RedirectToAction("Index", "Home");   // TODO: Redirect to "successfully registered" page
                }

                ModelState.AddModelError(string.Empty, "User with such Login or Email is already registered!");
            }
            
            return View(model);
        }
    }
}