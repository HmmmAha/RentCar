using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.API.Models
{
    public class MsCar
    {
        [Key]
        [StringLength(36)]
        public string Car_id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Model { get; set; }

        public int Year { get; set; }

        [StringLength(50)]
        public string License_plate { get; set; }

        public int Number_of_car_seats { get; set; }

        [StringLength(100)]
        public string Transmission { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price_per_day { get; set; }

        public bool Status { get; set; }

        // Navigation properties
        public virtual ICollection<TrRental> Rentals { get; set; }
        public virtual ICollection<TrMaintenance> Maintenances { get; set; }
        public virtual ICollection<MsCarImages> CarImages { get; set; }
    }
}
