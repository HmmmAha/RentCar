using Microsoft.AspNetCore.Mvc;
using RentCar.WebClient.Models;
using RentCar.WebClient.Models.Cart;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

using RentCar.WebClient.Models.History;

namespace RentCar.WebClient.Controllers
{
    public class HistoryController : Controller
    {
        private readonly HttpClient _http;

        public HistoryController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri("https://localhost:7261/");
        }

        // GET: History - Shows all rentals
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



                var response = await _http.GetAsync($"api/History/{customerId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var historyResponse = JsonSerializer.Deserialize<RentalHistoryResponseDto>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return View(historyResponse);
                }
                else
                {
                    return View(new RentalHistoryResponseDto
                    {
                        Rentals = new List<RentalHistoryDto>(),
                        TotalRentals = 0,
                        PaidRentals = 0,
                        UnpaidRentals = 0
                    });
                }
            }
            catch (Exception ex)
            {
                return View(new RentalHistoryResponseDto
                {
                    Rentals = new List<RentalHistoryDto>(),
                    TotalRentals = 0,
                    PaidRentals = 0,
                    UnpaidRentals = 0
                });
            }
        }
    }
}
