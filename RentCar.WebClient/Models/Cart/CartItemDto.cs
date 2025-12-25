namespace RentCar.WebClient.Models.Cart
{
    public class CartItemDto
    {
        public string Rental_id { get; set; }
        public string Customer_id { get; set; }
        public string Car_id { get; set; }
        public DateTime Rental_date { get; set; }
        public DateTime Return_date { get; set; }
        public decimal Total_price { get; set; }

        // Car details
        public string Car_name { get; set; }
        public string Car_model { get; set; }
        public int Car_year { get; set; }
        public decimal Price_per_day { get; set; }
        public int Rental_days { get; set; }
    }
}
