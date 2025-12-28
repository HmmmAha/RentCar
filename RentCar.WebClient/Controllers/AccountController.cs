using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public IActionResult Login()
        {
            // redirects to home if user is logged in
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // safety net just in case
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var response = await _http.PostAsJsonAsync("api/Auth/login", model);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        ViewBag.Error = "Email atau password salah";
                    }
                    else
                    {
                        ViewBag.Error = "Login gagal. Silakan coba lagi.";
                    }
                    return View(model);
                }

                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

                if (loginResponse is null)
                {
                    ViewBag.Error = "Login gagal. Silakan coba lagi.";
                    throw new Exception("Login response was null");
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, loginResponse.Customer_id),
                    new Claim(ClaimTypes.Email, loginResponse.Email),
                    new Claim(ClaimTypes.Name, loginResponse.Name)
                };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                var principal = new ClaimsPrincipal(identity);


                // AUTH PERSISTENCE
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                    });

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Terjadi kesalahan. Silakan coba lagi.";
                return View(model);
            }

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            // redirects to home if user is logged in
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // safety nets just in case
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }


            try
            {
                var response = await _http.PostAsJsonAsync("api/Auth/register", model);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        ViewBag.Error = errorContent.Replace("\"", ""); // Remove quotes if JSON string
                    }
                    else
                    {
                        ViewBag.Error = "Registrasi gagal. Silakan coba lagi.";
                    }

                    return View(model);
                }

                return RedirectToAction("Login");

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Terjadi kesalahan. Silakan coba lagi.";
                return View(model);
            }


        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );
            return RedirectToAction("Index", "Home");
        }

    }
}
