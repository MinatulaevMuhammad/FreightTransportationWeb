using System.ComponentModel.DataAnnotations;

namespace FreightTransportationWeb.Models
{
    public class DeliveryAddress
    {
        [Key]
        public int Id { get; set; }
        public string House { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
    }
}
