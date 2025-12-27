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
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password minimal 8 karakter")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password dan konfirmasi password tidak sama")]
        public string Confirm_password { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Nomor telepon hanya boleh angka")]
        public string Phone_number { get; set; }

        [Required]
        public string Driver_license_number { get; set; }
    }
}
