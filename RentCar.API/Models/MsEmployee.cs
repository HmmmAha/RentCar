using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.API.Models
{
    public class MsEmployee
    {
        [Key]
        [StringLength(36)]
        public string Employee_id { get; set; }

        [Required]
        public DateTime Name { get; set; }

        [StringLength(4000)]
        [Required]
        public string Position { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Email { get; set; }

        [StringLength(36)]
        [Required]
        public string Phone_number { get; set; }

        // Navigation properties
        public virtual ICollection<TrMaintenance> Maintenances { get; set; }
    }
}
