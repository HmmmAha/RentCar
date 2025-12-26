using Microsoft.AspNetCore.Mvc;
using RentCar.WebClient.Models;
using RentCar.WebClient.Models.Cart;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace RentCar.WebClient.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _http;

        public CartController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri("https://localhost:7261/");
        }

        // GET: Cart - Shows unpaid rentals
        public async Task<IActionResult> Index()
        {
            try
            {
                // Get customer ID from cookie
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(customerId))
                {
                    return RedirectToAction("Login", "Account");
                }



                var response = await _http.GetAsync($"api/Rental/cart/{customerId}");

                Console.WriteLine("response: " + response);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var cartResponse = JsonSerializer.Deserialize<CartResponseDto>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return View(cartResponse);
                }

                return View(new CartResponseDto { Items = new List<CartItemDto>(), TotalItems = 0 });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error loading cart: {ex.Message}";
                return View(new CartResponseDto { Items = new List<CartItemDto>(), TotalItems = 0 });
            }
        }

        // POST: Cart/Add - Creates rental with payment_status = false
        [HttpPost]
        public async Task<IActionResult> Add(AddToCartRequest request)
        {
            Console.WriteLine("hahaha");
            try
            {
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                {
                    return Json(new { success = false, message = "Please login first" });
                }


                var createRentalDto = new
                {
                    Customer_id = customerId,
                    Car_id = request.Car_id,
                    Rental_date = request.Rental_date,
                    Return_date = request.Return_date
                };

                var json = JsonSerializer.Serialize(createRentalDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync($"api/Rental/create", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return Json(new { success = false, message = errorResponse?.Message ?? "Failed to add to cart" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Cart/Remove/{id} - Deletes unpaid rental
        [HttpPost]
        public async Task<IActionResult> Remove(string id)
        {
            try
            {

                var response = await _http.DeleteAsync($"api/Rental/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Removed from cart" });
                }

                return Json(new { success = false, message = "Failed to remove item" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Cart/Pay - Updates payment_status to true and creates payment record
        [HttpPost]
        public async Task<IActionResult> Pay([FromBody] PaymentRequest request)
        {
            try
            {
                var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                {
                    return Json(new { success = false, message = "Please login first" });
                }

                var payRentalDto = new
                {
                    Customer_id = customerId,
                    Rental_id = request.Rental_id,
                    Payment_method = request.Payment_method
                };

                var json = JsonSerializer.Serialize(payRentalDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync($"api/Rental/pay", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var paymentResponse = JsonSerializer.Deserialize<CheckoutResponseDto>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return Json(new
                    {
                        success = true,
                        message = paymentResponse.Message,
                        rentalId = paymentResponse.Rental_id
                    });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return Json(new { success = false, message = errorResponse?.Message ?? "Payment failed" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        // REQUESTS AND RESPONSES
        public class AddToCartRequest
        {
            public string Car_id { get; set; }
            public DateTime Rental_date { get; set; }
            public DateTime Return_date { get; set; }
        }

        public class PaymentRequest
        {
            public string Rental_id { get; set; }
            public string Payment_method { get; set; }
        }

        public class ErrorResponse
        {
            public string Message { get; set; }
        }


        // DTOs
        public class CheckoutResponseDto
        {
            public string Rental_id { get; set; }
            public string Payment_id { get; set; }
            public decimal Total_price { get; set; }
            public string Message { get; set; }
        }
        
    }
}
