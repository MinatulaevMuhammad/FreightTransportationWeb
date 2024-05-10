using FreightTransportationWeb.Models;

namespace FreightTransportationWeb.ViewModels
{
    public class EditUserDashboardViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public IFormFile? Image {  get; set; }
        public string? SaveImage { get; set; }
        public string? PhoneNumber { get; set; }
        public AddressUser AddressUser { get; set; }
    }
}
