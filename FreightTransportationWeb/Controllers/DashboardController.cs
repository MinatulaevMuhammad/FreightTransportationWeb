using FreightTransportationWeb.Data;
using FreightTransportationWeb.Interfaces;
using FreightTransportationWeb.Models;
using FreightTransportationWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FreightTransportationWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public void UserEdit(AppUser appUser, EditUserDashboardViewModel editVM)
        {
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
                AddressUser = user.Address
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
