using FreightTransportationWeb.Data;
using FreightTransportationWeb.Models;
using FreightTransportationWeb.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FreightTransportationWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _userManager=userManager;
            _signInManager=signInManager;
            _context=context;
            _webHostEnvironment = webHostEnvironment;
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

        [HttpGet]
        public IActionResult Register()
        {
            var repsonse = new RegisterViewModel();
            return View(repsonse);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerViewModel);
            }

            string uniqueFileName = null;
            if (registerViewModel.Image != null)
            {
                uniqueFileName = Guid.NewGuid().ToString() + "_" + registerViewModel.Image.FileName;
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await registerViewModel.Image.CopyToAsync(fileStream);
                }
            }

            var newUser = new AppUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.UserName,
                Address = registerViewModel.AddressUser,
                Image = uniqueFileName,
                PhoneNumber = registerViewModel.PhoneNumber
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if(newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
