using FreightTransportationWeb.Models;

namespace FreightTransportationWeb.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAll();
        Task<Order> GetByIdAsync(int id);
        Task<Order> GetByIdAsyncNoTracking(int id);
        Task<IEnumerable<Order>> GetOrderByCity(string city);
        bool Add(Order order);
        bool Update(Order order);
        bool Delete(Order order, DeliveryAddress deliveryAddress, Package package);
        bool Save();
    }
}
