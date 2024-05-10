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
        private readonly ApplicationDbContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public OrderController(ApplicationDbContext context, IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _context = context;
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
                    Price = orderVM.Price,
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
                Price = order.Price,
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
                    Price = orderVM.Price,
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
        public async Task<IActionResult> SubmitApplication(int id)
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            if (_context.Auctions.FirstOrDefault(a => a.OrderId == id && a.ContractorId == _httpContextAccessor.HttpContext.User.GetUserId()) != null) 
            {
                TempData["ErrorMessage"] = "Ошибка: Вы уже оставили заявку на данный заказ.";
                return RedirectToAction("Index"); // Перенаправление на страниц
            }
            Order order = await _orderRepository.GetByIdAsync(id);
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitApplication(int id, int Price)
        {
            Order order = await _orderRepository.GetByIdAsync(id);
            Auction auction = new Auction
            {
                OrderId = id,
                Price = Price,
                ContractorId = _httpContextAccessor.HttpContext.User.GetUserId()
            };

            _context.Add(auction);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ViewApplications(int id)
        {
            List <AuctionViewModel> applicationsInfo = new List<AuctionViewModel>();
            var applications = await _context.Auctions.Where(c => c.OrderId == id).ToListAsync();

            foreach(var app in applications)
            {
                var contractor = await _userRepository.GetUserById(app.ContractorId);

                var commentsUser = _context.Comments.Where(a => a.AppUserCommentId == app.ContractorId).ToList();
                double averageRating = 0;
                if (commentsUser.Count != 0) averageRating = commentsUser.Sum(r => r.Rating) / (double)commentsUser.Count;
                var applicationInfo = new AuctionViewModel
                {
                    Id = app.Id,
                    AverageRating = averageRating,
                    Price = app.Price,
                    Contractor = contractor
                };
                applicationsInfo.Add(applicationInfo);
            }

            return View(applicationsInfo);
        }

        [HttpGet]
        public async Task<IActionResult> ChoosContractor(int id)
        {
            var application = _context.Auctions.FirstOrDefault(c => c.Id == id);

            Order order = await _orderRepository.GetByIdAsync(application.OrderId);

            order.ContractorId = application.ContractorId;
            order.OrderStatus = OrderStatus.InProgress;
            order.Price = application.Price;
            _orderRepository.Update(order);

            var applications = _context.Auctions.Where(a => a.OrderId == order.Id).ToList();
            foreach(var app in applications)
            {
                _context.Auctions.Remove(app);
                _context.SaveChanges();
            }

            return RedirectToAction("Index","Dashboard");
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
