using Microsoft.AspNetCore.Mvc;
using RentCar.API.Data;
using RentCar.API.Models;
using RentCar.API.DTOs.Auth;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace RentCar.API.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool emailExists = await _context.MsCustomers
                .Where(c => c.Email == dto.Email)
                .AnyAsync();

            if (emailExists)
                return BadRequest("Email already registered");


            Console.WriteLine(dto);
            MsCustomer customer = new MsCustomer
            {
                Customer_id = Guid.NewGuid().ToString(),
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Name = dto.Name,
                Phone_number = dto.Phone_number,
                Address = dto.Email,
                Driver_license_number = dto.Driver_license_number
            };

            await _context.MsCustomers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Register successful"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            MsCustomer user = await _context.MsCustomers
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

            return Ok(new
            {
                user.Customer_id,
                user.Name,
                user.Email
            });
        }
    }
}
