using FreightTransportationWeb.Data;
using FreightTransportationWeb.Models;
using FreightTransportationWeb.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FreightTransportationWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,ApplicationDbContext context)
        {
            _userManager=userManager;
            _signInManager=signInManager;
            _context=context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var repsonse = new LoginViewModel();
            return View(repsonse);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if(result.Succeeded) 
                    {
                        return RedirectToAction("Index", "Order");
                    }
                }
                TempData["Error"] = "Введен неправильный пароль. Пожалуйста, попробуйте снова!";
                return View(loginViewModel);
            }
            TempData["Error"] = "Данный пользователь не обнаружен. Пожалуйста, попробуйте снова!";
            return View(loginViewModel);
        }
    }
}
