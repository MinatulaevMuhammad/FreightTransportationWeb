using FreightTransportationWeb.Models;

namespace FreightTransportationWeb.ViewModels
{
    public class UserViewModel
    {
        public string Id {  get; set; }
        public string UserName { get; set; }
        public AddressUser AddressUser { get; set; }
    }
}
