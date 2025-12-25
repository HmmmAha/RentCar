namespace RentCar.WebClient.Models.Cars
{
    public class CarAPIResponse
    {
        public List<CarDto> Cars { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCars { get; set; }
        public int PageSize { get; set; }
    }
}
