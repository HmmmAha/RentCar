namespace RentCar.WebClient.Models.Auth
{
    public class LoginResponseDto
    {
        public required string Customer_id { get; set; }
        public required string Email { get; set; }
        public required string Name { get; set; }
    }
}
