using FreightTransportationWeb.Data.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightTransportationWeb.Models
{
    public class AppUser: IdentityUser
    {
        public UserRole UserRole { get; set; }
        [ForeignKey ("AddressUser")]
        public int AddressId { get; set; }
        public AddressUser Address { get; set; }
    }
}
