using FreightTransportationWeb.Models;

namespace FreightTransportationWeb.ViewModels
{
    public class AuctionViewModel
    {
        public int Id { get; set; }
        public double AverageRating { get; set; }
        public int Price { get; set; }
        public AppUser Contractor { get; set; }
    }
}
