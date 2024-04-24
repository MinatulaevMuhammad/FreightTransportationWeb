using System.ComponentModel.DataAnnotations;

namespace FreightTransportationWeb.Models
{
    public class Package
    {
        [Key]
        public int Id { get; set; }
        public int Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string? Description { get; set; }
    }
}
