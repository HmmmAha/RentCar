using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.API.Models
{
    public class LtPayment
    {
        [Key]
        [StringLength(36)]
        public string Payment_id { get; set; }

        [ForeignKey("Rental")]
        [StringLength(36)]
        public string Rental_id { get; set; }

        public DateTime Payment_date { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [StringLength(100)]
        public string Payment_method { get; set; }

        // Navigation property
        public virtual TrRental Rental { get; set; }
    }
}
