using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.API.Data;
using RentCar.API.DTOs.History;

namespace RentCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HistoryController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/History/{customerId}
        [HttpGet("{customerId}")]
        public async Task<ActionResult<RentalHistoryResponseDto>> GetRentalHistory(string customerId)
        {
            try
            {
                var rentals = await _context.TrRentals
                    .Include(r => r.Car)
                    .Where(r => r.Customer_id == customerId)
                    .OrderByDescending(r => r.Rental_date)
                    .Select(r => new RentalHistoryDto
                    {
                        Rental_id = r.Rental_id,
                        Rental_date = r.Rental_date,
                        Return_date = r.Return_date,
                        Car_name = r.Car.Name,
                        Car_model = r.Car.Model,
                        Car_year = r.Car.Year,
                        Price_per_day = r.Car.Price_per_day,
                        Total_price = r.Total_price,
                        Payment_status = r.Payment_status
                    })
                    .ToListAsync();

                var response = new RentalHistoryResponseDto
                {
                    Rentals = rentals,
                    TotalRentals = rentals.Count,
                    PaidRentals = rentals.Count(r => r.Payment_status),
                    UnpaidRentals = rentals.Count(r => !r.Payment_status)
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching rental history", error = ex.Message });
            }
        }
    }
}
