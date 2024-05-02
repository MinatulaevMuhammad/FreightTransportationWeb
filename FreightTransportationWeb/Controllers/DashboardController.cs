using FreightTransportationWeb.Data;
using FreightTransportationWeb.Interfaces;
using FreightTransportationWeb.Models;
using FreightTransportationWeb.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FreightTransportationWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }
        public async void UserEdit(AppUser appUser, EditUserDashboardViewModel editVM)
        {
            if (editVM.Image != null)
            {
                string oldPhotoPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", appUser.Image);

                if (System.IO.File.Exists(oldPhotoPath))
                {
                    System.IO.File.Delete(oldPhotoPath);
                }

                string newUniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(editVM.Image.FileName);
                string newFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", newUniqueFileName);
                using (var fileStream = new FileStream(newFilePath, FileMode.Create))
                {
                    await editVM.Image.CopyToAsync(fileStream);
                }

                appUser.Image = newUniqueFileName;
            }
            appUser.Id = editVM.Id;
            appUser.Address = editVM.AddressUser;
        }
        public async Task<IActionResult> Index()
        {
            var userOrders = await _dashboardRepository.GetAllUserOrders();
            var userViewModel = new DashboardViewModel() 
            {
                Orders = userOrders
            };
            return View(userViewModel);
        }

        [HttpGet] 
        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(curUserId);
            if(user == null)
            {
                return View("Error");
            }
            var editUserViewModel = new EditUserDashboardViewModel()
            {
                Id = curUserId,
                UserName = user.UserName,
                AddressUser = user.Address,
                SaveImage = user.Image
            };
            return View(editUserViewModel);
        }
		[HttpPost]
		public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile",editVM);
            }

            var user = await _dashboardRepository.GetUserByIdNoTracking(editVM.Id);

            if(user != null )
            {
                UserEdit(user, editVM);

                _dashboardRepository.Update(user);

                return RedirectToAction("Index");
            }
            else
            {
                return View(editVM);
            }
        }

        public async Task<IActionResult> AcceptedOrders()
        {
            var contractorOrders = await _dashboardRepository.GetAllContractorOrders();
            var userViewModel = new DashboardViewModel()
            {
                Orders = contractorOrders
            };
            return View(userViewModel);
        }

    }
}
