using FreightTransportationWeb.Data;
using FreightTransportationWeb.Interfaces;
using FreightTransportationWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreightTransportationWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;

        public UserController(ApplicationDbContext context,IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var commentsUser = _context.Comments.Where(a => a.AppUserCommentId == user.Id).ToList();
                double averageRating = 0;

                if (commentsUser.Count != 0)
                {
                    averageRating = commentsUser.Sum(r => r.Rating) / (double)commentsUser.Count;
                }

                var userViewModel = new UserViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    AddressUser = user.Address,
                    AverageRating = averageRating
                };
                result.Add(userViewModel);
            }
            return View(result);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepository.GetUserById(id);
            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                AddressUser = user.Address,
                Image = user.Image
            };

            return View(userDetailViewModel);
        }
    }
}
