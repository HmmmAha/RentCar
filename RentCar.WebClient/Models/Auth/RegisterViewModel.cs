using System.ComponentModel.DataAnnotations;

namespace RentCar.WebClient.Models.Auth
{
    public class RegisterViewModel
    {
        [Required]
        public required string Name { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(14, ErrorMessage = "Password minimal 14 karakter")]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password dan konfirmasi password tidak sama")]
        public required string Confirm_password { get; set; }

        [Required]
        public required string Address { get; set; }

        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Nomor telepon hanya boleh angka")]
        public required string Phone_number { get; set; }

        [Required]
        public required string Driver_license_number { get; set; }
    }
}
