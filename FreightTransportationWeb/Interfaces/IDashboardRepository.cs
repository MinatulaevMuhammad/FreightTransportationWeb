using FreightTransportationWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace FreightTransportationWeb.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Order>> GetAllUserOrders();
        Task<List<Order>> GetAllContractorOrders();
        Task<List<Order>> GetAllContractorAuctions();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetUserByIdNoTracking(string id);
        bool Update(AppUser appUser);
        public bool Save();

	}
}
