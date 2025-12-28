using System.ComponentModel.DataAnnotations;

namespace RentCar.API.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        public required string Name { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string Address { get; set; }

        public required string Phone_number { get; set; }

        public required string Driver_license_number { get; set; }


    }
}
