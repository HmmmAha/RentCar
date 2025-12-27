using System.ComponentModel.DataAnnotations;

namespace RentCar.WebClient.Models.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email wajib diisi")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password wajib diisi")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
