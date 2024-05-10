using System.ComponentModel.DataAnnotations;

namespace FreightTransportationWeb.Models
{
    public class Auction
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ContractorId { get; set; }
        public int Price { get; set; }
    }
}
