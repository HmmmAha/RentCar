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
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(4000)]
        [Required]
        public string Position { get; set; }

        [StringLength(100)]
        [Required]
        public string Email { get; set; }

        [StringLength(50)]
        [Required]
        public string Phone_number { get; set; }

        // Navigation properties
        public virtual ICollection<TrMaintenance> Maintenances { get; set; }
    }
}
