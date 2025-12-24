using System.ComponentModel.DataAnnotations;

namespace RentCar.API.Models
{
    public class MsCustomer
    {
        [Key]
        [StringLength(36)]  // Assuming UUID format
        public string Customer_id { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Phone_number { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Driver_license_number { get; set; }


        // Navigation properties
        public virtual ICollection<TrRental> Rentals { get; set; }
    }
}
