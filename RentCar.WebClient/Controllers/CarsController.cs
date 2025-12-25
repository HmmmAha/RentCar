using Microsoft.AspNetCore.Mvc;
using RentCar.WebClient.Models.Cars;
using System.Net.Http;
using System.Text.Json;

namespace RentCar.WebClient.Controllers
{
    public class CarsController : Controller
    {
        private readonly HttpClient _http;

        public CarsController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri("https://localhost:7261/");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            Console.WriteLine("id: ", id);
            try
            {

                var response = await _http.GetAsync($"api/Cars/{id}");
                Console.WriteLine("done fetching:", response);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("success");
                    var content = await response.Content.ReadAsStringAsync();
                    var car = JsonSerializer.Deserialize<CarDto>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return View(car);
                }
                else
                {
                    Console.WriteLine("not success");
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
