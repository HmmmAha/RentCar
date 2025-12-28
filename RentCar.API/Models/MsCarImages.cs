using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentCar.API.Models
{
    public class MsCarImages
    {
        [Key]
        [StringLength(36)]
        public string Image_car_id { get; set; }

        [ForeignKey("Car")]
        [StringLength(36)]
        [Required]
        public string Car_id { get; set; }

        [StringLength(2000)]
        [Required]
        public string Image_link { get; set; }



        public virtual MsCar Car { get; set; }
    }
}
