using FreightTransportationWeb.Data;
using FreightTransportationWeb.Interfaces;
using FreightTransportationWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace FreightTransportationWeb.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Order order)
        {
            _context.Add(order);
            return Save();
        }
        public bool Delete(Order order, DeliveryAddress deliveryAddress, Package package)
        {
            _context.Remove(order);
            _context.Remove(deliveryAddress);
            _context.Remove(package);
            return Save();
        }
        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _context.Orders.Include(c => c.Package)
                                        .Include(d => d.DeliveryAddress)
                                        .Include(c => c.Customer.Address)
                                        .ToListAsync();
        }
        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders.Include(c => c.Customer)
                                        .Include(c => c.Customer.Address)
                                        .Include(a => a.DeliveryAddress)
                                        .Include(p => p.Package)
                                        .FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<Order> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Orders.Include(c => c.Customer)
                                        .Include(c => c.Customer.Address)
                                        .Include(a => a.DeliveryAddress)
                                        .Include(p => p.Package)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<IEnumerable<Order>> GetOrderByCity(string city)
        {
            return await _context.Orders.Where(c=>c.Customer.Address.City.Contains(city)).ToListAsync();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return  saved > 0 ? true: false;
        }
        public bool Update(Order order)
        {
            _context.Update(order);
            return Save();
        }
    }
}
