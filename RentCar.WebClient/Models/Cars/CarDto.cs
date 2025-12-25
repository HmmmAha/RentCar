namespace RentCar.WebClient.Models.Cars
{
    public class CarDto
    {
        public string Car_id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string License_plate { get; set; }
        public int Number_of_car_seats { get; set; }
        public string Transmission { get; set; }
        public decimal Price_per_day { get; set; }
        public bool Status { get; set; }
        public List<CarImageDto> CarImages { get; set; }
    }
}
