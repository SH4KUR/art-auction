using ArtAuction.WebUI.Models.Account;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.WebUI.Controllers
{
    public class AccountController : Controller
    {
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
        public IActionResult Registration(AccountRegistrationViewModel model)
        {
            try
            {
                return RedirectToAction(nameof(Registration));
            }
            catch
            {
                return View();
            }
        }
    }
}