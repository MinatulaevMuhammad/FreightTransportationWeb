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

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Order orderVM)
        {
            try
            {
                var order = new Order
                {
                    Customer = new AppUser
                    {
                        UserName = "Jhon",
                        UserRole = UserRole.Customer,
                        Address = new AddressUser
                        {
                            House = "5",
                            Street = "SDASd",
                            City = "sdgds"
                        }
                    },
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
    }
}
