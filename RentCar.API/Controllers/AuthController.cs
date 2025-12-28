using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.API.Data;
using RentCar.API.DTOs.Auth;
using RentCar.API.Models;

namespace RentCar.API.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost("api/Auth/register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool emailExists = await _context.MsCustomers
                .Where(c => c.Email == dto.Email)
                .AnyAsync();

            if (emailExists)
                return BadRequest("Email sudah terdaftar.");

            var customer = new MsCustomer
            {
                Customer_id = Guid.NewGuid().ToString(),
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Name = dto.Name,
                Phone_number = dto.Phone_number,
                Address = dto.Address,
                Driver_license_number = dto.Driver_license_number
            };

            await _context.MsCustomers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Register successful"
            });
        }

        [HttpPost("api/Auth/login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.MsCustomers
                .Where(c => c.Email == dto.Email)
                .Select(c => new MsCustomer
                {
                    Customer_id = c.Customer_id,
                    Email = c.Email,
                    Password = c.Password,
                    Name = c.Name
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return Unauthorized("Invalid email or password");

            bool passwordValid =
                BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            if (!passwordValid)
                return Unauthorized("Invalid email or password");

            return Ok(new LoginResponse
            {
                Customer_id = user.Customer_id,
                Name = user.Name,
                Email = user.Email
            });
        }
    }

    public class LoginResponse
    {
        public required string Customer_id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
