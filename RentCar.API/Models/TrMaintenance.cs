using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.API.Models
{
    public class TrMaintenance
    {
        [Key]
        [StringLength(36)]
        public string Maintenance_id { get; set; }

        [ForeignKey("Car")]
        [StringLength(36)]
        public string Car_id { get; set; }

        [ForeignKey("Employee")]
        [StringLength(36)]
        public string Employee_id { get; set; }

        public DateTime Maintenance_date { get; set; }

        [StringLength(4000)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }

        // Navigation properties
        public virtual MsCar Car { get; set; }
        public virtual MsEmployee Employee { get; set; }
    }
}
