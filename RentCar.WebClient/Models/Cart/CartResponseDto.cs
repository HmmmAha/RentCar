namespace RentCar.WebClient.Models.Cart
{
    public class CartResponseDto
    {
        public List<CartItemDto> Items { get; set; }
        public int TotalItems { get; set; }
    }
}
