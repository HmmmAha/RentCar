using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RentCar.WebClient.Models.Auth;
using System.Security.Claims;
namespace RentCar.WebClient.Controllers
{
    public class AccountController : Controller
    {
        
        private readonly HttpClient _http;

        public AccountController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient();
            _http.BaseAddress = new Uri("https://localhost:7261/");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var response = await _http.PostAsJsonAsync("api/Auth/login", model);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Login failed";
                return View();
            }

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();


            HttpContext.Session.SetString("Email", loginResponse.Email);
            HttpContext.Session.SetString("CustomerId", loginResponse.Customer_id);
            HttpContext.Session.SetString("Name", loginResponse.Name);


            Console.WriteLine("returning");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var response = await _http.PostAsJsonAsync("api/Auth/register", model);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Register failed";
                return View();
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        
    }
}
