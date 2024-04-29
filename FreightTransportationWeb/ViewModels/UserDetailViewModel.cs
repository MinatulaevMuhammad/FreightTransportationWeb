using FreightTransportationWeb.Models;

namespace FreightTransportationWeb.ViewModels
{
    public class UserDetailViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public AddressUser AddressUser { get; set; }
    }
}
