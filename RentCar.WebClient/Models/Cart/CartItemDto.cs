namespace RentCar.WebClient.Models.Cart
{
    public class CartItemDto
    {
        public required string Rental_id { get; set; }
        public required string Customer_id { get; set; }
        public required string Car_id { get; set; }
        public DateTime Rental_date { get; set; }
        public DateTime Return_date { get; set; }
        public decimal Total_price { get; set; }

        // Car details
        public required string Car_name { get; set; }
        public required string Car_model { get; set; }
        public int Car_year { get; set; }
        public decimal Price_per_day { get; set; }
        public int Rental_days { get; set; }
    }
}
