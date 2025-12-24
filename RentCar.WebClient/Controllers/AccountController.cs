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
            var response = await _http.PostAsJsonAsync("login", model);

            if (!response.IsSuccessStatusCode)
            {
                
                ViewBag.Error = "Login failed";
                return View();
            }


            HttpContext.Session.SetString("Email", model.Email);
            Console.WriteLine("returning");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var response = await _http.PostAsJsonAsync("register", model);

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
