using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.API.Models
{
    public class TrRental
    {
        [Key]
        [StringLength(36)]
        public string Rental_id { get; set; }

        [ForeignKey("Customer")]
        [StringLength(36)]
        public string Customer_id { get; set; }

        [ForeignKey("Car")]
        [StringLength(36)]
        public string Car_id { get; set; }

        public DateTime Rental_date { get; set; }

        public DateTime Return_date { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total_price { get; set; }

        public bool Payment_status { get; set; }


        public virtual MsCustomer Customer { get; set; }
        public virtual MsCar Car { get; set; }
        public virtual ICollection<LtPayment> Payments { get; set; }
    }
}
