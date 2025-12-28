namespace RentCar.WebClient.Models.Cars
{
    public class CarsViewModel
    {
        public required List<CarDto> Cars { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public required string SortBy { get; set; }
        public required string SortOrder { get; set; }
        public int TotalCars { get; set; }

        // default to valid to avoid accidental invalid states
        public CarValidationStatus CarValidation { get; set; } = CarValidationStatus.Valid;
        public int? YearFilter { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
