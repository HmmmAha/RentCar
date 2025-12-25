using Microsoft.AspNetCore.Mvc;
using RentCar.WebClient.Models.Cars;
using System.Net.Http;
using System.Text.Json;

namespace RentCar.WebClient.Controllers
{
    public class CarController : Controller
    {
        private readonly HttpClient _http;

        public CarController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri("https://localhost:7261/");
        }

        [HttpGet]
        public async Task<IActionResult> Details(
            string id,
            DateTime? rentDate,
            DateTime? returnDate
        )
        {
            try
            {

                var response = await _http.GetAsync($"api/Car/{id}");
                Console.WriteLine("done fetching:", response);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("success");
                    var content = await response.Content.ReadAsStringAsync();
                    var car = JsonSerializer.Deserialize<CarDto>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    ViewBag.RentDate = rentDate;
                    ViewBag.ReturnDate = returnDate;

                    return View(car);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View("Error");
            }
        }
    }
}
