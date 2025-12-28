namespace RentCar.API.DTOs.Rentals
{
    public class CreateRentalDto
    {
        public required string Customer_id { get; set; }
        public required string Car_id { get; set; }
        public DateTime Rental_date { get; set; }
        public DateTime Return_date { get; set; }
    }

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

    public class CartResponseDto
    {
        public required List<CartItemDto> Items { get; set; }
        public int TotalItems { get; set; }
    }

    public class PayRentalDto
    {
        public required string Customer_id { get; set; }
        public required string Rental_id { get; set; }
        public required string Payment_method { get; set; }
    }

    public class RentalResponseDto
    {
        public required string Rental_id { get; set; }
        public required string Message { get; set; }
    }

    public class CheckoutResponseDto
    {
        public required string Rental_id { get; set; }
        public required string Payment_id { get; set; }
        public required decimal Total_price { get; set; }
        public required string Message { get; set; }
    }
}
