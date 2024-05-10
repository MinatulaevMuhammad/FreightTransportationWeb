using FreightTransportationWeb.Models;

namespace FreightTransportationWeb.ViewModels
{
    public class DashboardViewModel
    {
        public List<Order> Auctions { get; set; }
        public List<Order> Orders { get; set; }
    }
}
