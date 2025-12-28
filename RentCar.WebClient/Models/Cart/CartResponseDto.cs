namespace RentCar.WebClient.Models.Cart
{
    public class CartResponseDto
    {
        public required List<CartItemDto> Items { get; set; }
        public int TotalItems { get; set; }
    }
}
