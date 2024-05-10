using FreightTransportationWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreightTransportationWeb.Data
{
    public class ApplicationDbContext: IdentityDbContext<AppUser>
    {
        private readonly DbContextOptions<ApplicationDbContext> options;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            this.options = options;
        }
        public DbSet<AddressUser> AddressesUser { get; set; }
        public DbSet<DeliveryAddress> DeliveryAddresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Auction> Auctions { get; set; }
    }
}
