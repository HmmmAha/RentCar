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

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
