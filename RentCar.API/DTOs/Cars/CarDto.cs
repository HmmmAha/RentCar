namespace RentCar.API.DTOs.Cars
{
    public class CarDto
    {
        public required string Car_id { get; set; }
        public required string Name { get; set; }
        public required string Model { get; set; }
        public required int Year { get; set; }
        public required string License_plate { get; set; }
        public required int Number_of_car_seats { get; set; }
        public required string Transmission { get; set; }
        public required decimal Price_per_day { get; set; }
        public bool Status { get; set; }
        public required List<CarImageDto> CarImages { get; set; }
    }
}
