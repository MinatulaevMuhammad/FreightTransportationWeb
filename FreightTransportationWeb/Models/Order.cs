using FreightTransportationWeb.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightTransportationWeb.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("AppUser")]
        public string? CustomerId { get; set; }
        public AppUser? Customer { get; set; }
        [ForeignKey("AppUser")]
        public string? ContractorId { get; set; }
        public AppUser? Contractor { get; set; }
        [ForeignKey("DeliveryAddress")]
        public int DeliveryAddressId { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }
        [ForeignKey("Package")]
        public int PackageId { get; set; }
        public Package Package { get; set; }
        public int Price { get; set; }
        public OrderStatus OrderStatus { get; set; }

    }
}
