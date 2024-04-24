using System.ComponentModel.DataAnnotations;

namespace FreightTransportationWeb.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name ="Email Address")]
        [Required]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
