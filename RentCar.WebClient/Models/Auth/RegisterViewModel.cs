using System.ComponentModel.DataAnnotations;

namespace RentCar.WebClient.Models.Auth
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Address { get; set; }

        public string Phone_number { get; set; }

        public string Driver_license_number { get; set; }
    }
}
