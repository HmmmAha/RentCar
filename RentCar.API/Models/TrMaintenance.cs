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
        [Required]
        public string Car_id { get; set; }

        [ForeignKey("Employee")]
        [StringLength(36)]
        [Required]
        public string Employee_id { get; set; }

        [Required]
        public DateTime Maintenance_date { get; set; }

        [StringLength(4000)]
        [Required]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Cost { get; set; }

        // Navigation properties
        public virtual MsCar Car { get; set; }
        public virtual MsEmployee Employee { get; set; }
    }
}
