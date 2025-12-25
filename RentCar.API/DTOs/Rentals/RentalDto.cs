namespace RentCar.API.DTOs.Rentals
{
    public class CreateRentalDto
    {
        public string Customer_id { get; set; }
        public string Car_id { get; set; }
        public DateTime Rental_date { get; set; }
        public DateTime Return_date { get; set; }
    }

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

    public class CartResponseDto
    {
        public List<CartItemDto> Items { get; set; }
        public int TotalItems { get; set; }
    }

    public class PayRentalDto
    {
        public string Customer_id { get; set; }
        public string Rental_id { get; set; }
        public string Payment_method { get; set; }
    }

    public class RentalResponseDto
    {
        public string Rental_id { get; set; }
        public string Message { get; set; }
    }

    public class CheckoutResponseDto
    {
        public string Rental_id { get; set; }
        public string Payment_id { get; set; }
        public decimal Total_price { get; set; }
        public string Message { get; set; }
    }
}
