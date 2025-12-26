namespace RentCar.API.DTOs.History
{
    public class RentalHistoryDto
    {
        public string Rental_id { get; set; }
        public DateTime Rental_date { get; set; }
        public DateTime Return_date { get; set; }

        // Car details
        public string Car_name { get; set; }
        public string Car_model { get; set; }
        public int Car_year { get; set; }

        // Pricing
        public decimal Price_per_day { get; set; }
        public decimal Total_price { get; set; }

        // Status
        public bool Payment_status { get; set; }
    }

    public class RentalHistoryResponseDto
    {
        public List<RentalHistoryDto> Rentals { get; set; }
        public int TotalRentals { get; set; }
        public int PaidRentals { get; set; }
        public int UnpaidRentals { get; set; }
    }
}
