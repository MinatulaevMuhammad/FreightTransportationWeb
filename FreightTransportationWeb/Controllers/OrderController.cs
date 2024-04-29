using FreightTransportationWeb.Data;
using FreightTransportationWeb.Data.Enum;
using FreightTransportationWeb.Interfaces;
using FreightTransportationWeb.Models;
using FreightTransportationWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightTransportationWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public OrderController(IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Order> orders = await _orderRepository.GetAll();
            return View(orders);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Order order = await _orderRepository.GetByIdAsync(id);
            return View(order);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var createOrderViewModel = new OrderViewModel { CustomerId = curUserId};
            return View(createOrderViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Order orderVM)
        {
            try
            {
                var order = new Order
                {
                    CustomerId = orderVM.CustomerId,
                    DeliveryAddress = new DeliveryAddress
                    {
                        House = orderVM.DeliveryAddress.House,
                        Street = orderVM.DeliveryAddress.Street,
                        City = orderVM.DeliveryAddress.City,
                        PhoneNumber = orderVM.DeliveryAddress.PhoneNumber
                    },
                    Package = new Package
                    {
                        Weight = orderVM.Package.Weight,
                        Length = orderVM.Package.Length,
                        Width = orderVM.Package.Width,
                        Height = orderVM.Package.Height,
                        Description = orderVM.Package.Description,
                    },
                    OrderStatus = OrderStatus.Created
                };
                _orderRepository.Add(order);
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Error creating");
            }
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return View("Error");
            var orderEdit = new OrderViewModel
            {
                Customer = order.Customer,
                CustomerId = order.CustomerId,
                DeliveryAddressId = order.DeliveryAddressId,
                DeliveryAddress = order.DeliveryAddress,
                PackageId = order.PackageId,
                Package = order.Package,
                OrderStatus = order.OrderStatus
            };
            return View(orderEdit);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, OrderViewModel orderVM)
        {
            var orderEdit = await _orderRepository.GetByIdAsyncNoTracking(id);
            if(orderEdit != null)
            {
                var order = new Order
                {
                    Id = id,
                    CustomerId = orderEdit.CustomerId,
                    Customer = orderEdit.Customer,
                    DeliveryAddressId = orderEdit.DeliveryAddressId,
                    DeliveryAddress = orderEdit.DeliveryAddress,
                    PackageId = orderEdit.PackageId,
                    Package = orderEdit.Package,
                    OrderStatus = orderEdit.OrderStatus
                };

                order.DeliveryAddress.PhoneNumber = orderVM.DeliveryAddress.PhoneNumber;
                order.DeliveryAddress.House = orderVM.DeliveryAddress.House;
                order.DeliveryAddress.Street = orderVM.DeliveryAddress.Street;
                order.DeliveryAddress.City = orderVM.DeliveryAddress.City;

                order.Package.Height = orderVM.Package.Height;
                order.Package.Length = orderVM.Package.Length;
                order.Package.Weight = orderVM.Package.Weight;
                order.Package.Width = orderVM.Package.Width;
                order.Package.Description = orderVM.Package.Description;

                _orderRepository.Update(order);
                return RedirectToAction("Index");
            }
            else
            {
                return View(orderVM);
            }
        }

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
        {
            var orderDetails = await _orderRepository.GetByIdAsync(id);
            if(orderDetails == null) return View("Error");
            return View(orderDetails);
        }
		[HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var orderDetail = await _orderRepository.GetByIdAsync(id);
            if( orderDetail == null) return View("Error");
            var deliveryAdress = orderDetail.DeliveryAddress;
            var package = orderDetail.Package;
            if (deliveryAdress == null || package == null) return View("Error");
            _orderRepository.Delete(orderDetail, deliveryAdress, package);
            return RedirectToAction("Index");

		}

        [HttpGet]
        public async Task<IActionResult> AcceptOrder(int id)
        {
            Order order = await _orderRepository.GetByIdAsync(id);
            var userId =  _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _userRepository.GetUserById(userId);
            if (order == null || userId == null || user == null) return View("Error");

            order.ContractorId = userId;
            order.OrderStatus = OrderStatus.InProgress;
            _orderRepository.Update(order);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Complete(int id)
        {
            Order order = await _orderRepository.GetByIdAsync(id);
            if (order == null ) return View("Error");

            order.OrderStatus = OrderStatus.Finished;
            _orderRepository.Update(order);
            return RedirectToAction("Create", "Comment", new {id = order.ContractorId});
        }

    }
}
