using Microsoft.AspNetCore.Mvc;
using RentCar.WebClient.Models;
using RentCar.WebClient.Models.Cars;
using System;
using System.Diagnostics;
using System.Text.Json;

namespace RentCar.WebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _http;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory factory)
        {
            _logger = logger;
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri("https://localhost:7261/");
        }

        [HttpGet]
        public async Task<ActionResult<CarsViewModel>> Index(
             DateTime? rentDate,
             DateTime? returnDate,
             int? yearFilter,
             int page = 1,
             string sortBy = "Name",
             string sortOrder = "asc")
        {
            if (!rentDate.HasValue || !returnDate.HasValue)
            {
                return View(new CarsViewModel
                {
                    Cars = new List<CarDto>(),
                    TotalCars = 0,
                    CurrentPage = page,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    CarValidation = CarValidationStatus.MissingDates
                });
            }

            if (rentDate >= returnDate)
            {
                return View(new CarsViewModel
                {
                    Cars = new List<CarDto>(),
                    TotalCars = 0,
                    CurrentPage = page,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    CarValidation = CarValidationStatus.InvalidDateRange
                });
            }

            if (rentDate < DateTime.Now.Date)
            {
                return View(new CarsViewModel
                {
                    Cars = new List<CarDto>(),
                    TotalCars = 0,
                    CurrentPage = page,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    CarValidation = CarValidationStatus.PastDate
                });
            }
            const int pageSize = 3;
            try
            {
                var url = $"/api/Car?rentDate={rentDate}&returnDate={returnDate}&page={page}&sortBy={sortBy}&sortOrder={sortOrder}";

                if (yearFilter.HasValue)
                {
                    url += $"&yearFilter={yearFilter}";
                }

                var response = await _http.GetAsync(url);

                
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = "Unable to fetch cars.";
                    return View(new CarsViewModel());
                }

                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<CarAPIResponse>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var viewModel = new CarsViewModel
                {
                    Cars = apiResponse?.Cars?.Take(pageSize).ToList() ?? new List<CarDto>(),
                    CurrentPage = apiResponse?.CurrentPage ?? 1,
                    TotalPages = apiResponse?.TotalPages ?? 0,
                    TotalCars = apiResponse?.TotalCars ?? 0,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    YearFilter = yearFilter
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(new CarsViewModel());
            }
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
