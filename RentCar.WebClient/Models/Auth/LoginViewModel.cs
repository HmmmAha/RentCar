using System.ComponentModel.DataAnnotations;

namespace RentCar.WebClient.Models.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email wajib diisi")]
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password wajib diisi")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
