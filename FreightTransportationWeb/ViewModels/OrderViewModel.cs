using FreightTransportationWeb.Data.Enum;
using FreightTransportationWeb.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightTransportationWeb.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public AppUser Customer { get; set; }
        public int? ContractorId { get; set; }
        public AppUser? Contractor { get; set; }
        public int DeliveryAddressId { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }
        public int PackageId { get; set; }
        public Package Package { get; set; }
        public int Price {  get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
