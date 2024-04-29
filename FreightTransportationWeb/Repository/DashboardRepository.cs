using FreightTransportationWeb.Data;
using FreightTransportationWeb.Interfaces;
using FreightTransportationWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FreightTransportationWeb.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Order>> GetAllUserOrders()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userOrders = _context.Orders.Where(i => i.Customer.Id == curUser).Include(p=>p.Package);
            return userOrders.ToList();
        }
        public async Task<List<Order>> GetAllContractorOrders()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userOrders = _context.Orders.Where(i => i.Contractor.Id == curUser).Include(p => p.Package);
            return userOrders.ToList();
        }
        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.Include(a => a.Address)
									   .FirstOrDefaultAsync(a => a.Id == id);
		}
        public async Task<AppUser> GetUserByIdNoTracking(string id)
        {
            return await _context.Users.Where(i => i.Id == id)
									   .AsNoTracking()
									   .FirstOrDefaultAsync();
        }
        public bool Update(AppUser appUser)
        {
            _context.Users.Update(appUser);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true: false;
        }

	}
}
