namespace RentCar.WebClient.Models.History
{
    public class RentalHistoryDto
    {
        public required string Rental_id { get; set; }
        public DateTime Rental_date { get; set; }
        public DateTime Return_date { get; set; }
        public required string Car_name { get; set; }
        public required string Car_model { get; set; }
        public int Car_year { get; set; }
        public decimal Price_per_day { get; set; }
        public decimal Total_price { get; set; }
        public bool Payment_status { get; set; }
    }

    public class RentalHistoryResponseDto
    {
        public required List<RentalHistoryDto> Rentals { get; set; }
        public int TotalRentals { get; set; }
        public int PaidRentals { get; set; }
        public int UnpaidRentals { get; set; }
    }
}
