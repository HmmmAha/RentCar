using Microsoft.AspNetCore.Components.Forms;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RentCar.WebClient.Models.Cars
{
    public class CarsViewModel
    {
        public List<CarDto> Cars { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public int TotalCars { get; set; }

        // default to valid to avoid accidental invalid states
        public CarValidationStatus CarValidation { get; set; } = CarValidationStatus.Valid;


        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
