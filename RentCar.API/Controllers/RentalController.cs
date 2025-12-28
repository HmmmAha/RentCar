using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.API.Data;
using RentCar.API.DTOs.Rentals;
using RentCar.API.Models;

namespace RentCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RentalController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Rental/cart/{customerId}
        [HttpGet("cart/{customerId}")]
        public async Task<ActionResult<CartResponseDto>> GetUnpaidRentals(string customerId)
        {
            try
            {
                var unpaidRentals = await _context.TrRentals
                    .Include(r => r.Car)
                        .ThenInclude(c => c.CarImages)
                    .Where(r => r.Customer_id == customerId && r.Payment_status == false)
                    .OrderByDescending(r => r.Rental_id)
                    .Select(r => new CartItemDto
                    {
                        Rental_id = r.Rental_id,
                        Customer_id = r.Customer_id,
                        Car_id = r.Car_id,
                        Rental_date = r.Rental_date,
                        Return_date = r.Return_date,
                        Total_price = r.Total_price,
                        Car_name = r.Car.Name,
                        Car_model = r.Car.Model,
                        Car_year = r.Car.Year,
                        Price_per_day = r.Car.Price_per_day,
                        Rental_days = (int)(r.Return_date - r.Rental_date).TotalDays
                    })
                    .ToListAsync();

                var response = new CartResponseDto
                {
                    Items = unpaidRentals,
                    TotalItems = unpaidRentals.Count
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching unpaid rentals", error = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<RentalResponseDto>> CreateRental([FromBody] CreateRentalDto dto)
        {
            try
            {
                // Validate dates
                if (dto.Rental_date >= dto.Return_date)
                {
                    return BadRequest(new { message = "Return date must be after rental date" });
                }

                if (dto.Rental_date < DateTime.Now.Date)
                {
                    return BadRequest(new { message = "Rental date cannot be in the past" });
                }

                // Check if car exists and is available
                var car = await _context.MsCars.FindAsync(dto.Car_id);
                if (car == null)
                {
                    return NotFound(new { message = "Car not found" });
                }

                if (!car.Status)
                {
                    return BadRequest(new { message = "Car is not available" });
                }

                // Check if car is already booked for these dates
                var conflictingBooking = await _context.TrRentals
                    .Where(r => r.Car_id == dto.Car_id &&
                                r.Payment_status == true &&
                                ((dto.Rental_date >= r.Rental_date && dto.Rental_date < r.Return_date) ||
                                 (dto.Return_date > r.Rental_date && dto.Return_date <= r.Return_date) ||
                                 (dto.Rental_date <= r.Rental_date && dto.Return_date >= r.Return_date)))
                    .AnyAsync();

                if (conflictingBooking)
                {
                    return BadRequest(new { message = "Car is already booked for these dates" });
                }

                // Check if customer already has unpaid rental for this car
                var existingUnpaid = await _context.TrRentals
                    .Where(r => r.Customer_id == dto.Customer_id &&
                                r.Car_id == dto.Car_id &&
                                r.Payment_status == false)
                    .AnyAsync();

                if (existingUnpaid)
                {
                    return BadRequest(new { message = "You already have an unpaid rental for this car in your cart" });
                }

                // Calculate total price
                var rentalDays = (dto.Return_date - dto.Rental_date).Days;
                var totalPrice = car.Price_per_day * rentalDays;

                // Create rental with payment_status = false
                var rental = new TrRental
                {
                    Rental_id = Guid.NewGuid().ToString(),
                    Customer_id = dto.Customer_id,
                    Car_id = dto.Car_id,
                    Rental_date = dto.Rental_date,
                    Return_date = dto.Return_date,
                    Total_price = totalPrice,
                    Payment_status = false
                };

                await _context.TrRentals.AddAsync(rental);
                await _context.SaveChangesAsync();

                return Ok(new RentalResponseDto
                {
                    Rental_id = rental.Rental_id,
                    Message = "Added to cart successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating rental", error = ex.Message });
            }
        }

        // DELETE: api/Rental/{rentalId}
        [HttpDelete("{rentalId}")]
        public async Task<ActionResult> DeleteUnpaidRental(string rentalId)
        {
            try
            {
                var rental = await _context.TrRentals
                    .FirstOrDefaultAsync(r => r.Rental_id == rentalId && r.Payment_status == false);

                if (rental == null)
                {
                    return NotFound(new { message = "Unpaid rental not found" });
                }

                _context.TrRentals.Remove(rental);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Rental removed from cart" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error removing rental", error = ex.Message });
            }
        }

        // POST: api/Rental/pay
        [HttpPost("pay")]
        public async Task<ActionResult<CheckoutResponseDto>> PayRental([FromBody] PayRentalDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Get unpaid rental
                var rental = await _context.TrRentals
                    .Include(r => r.Car)
                    .FirstOrDefaultAsync(r => r.Rental_id == dto.Rental_id &&
                                              r.Customer_id == dto.Customer_id &&
                                              r.Payment_status == false);

                if (rental == null)
                {
                    return NotFound(new { message = "Unpaid rental not found" });
                }

                // Check if car is still available
                if (!rental.Car.Status)
                {
                    return BadRequest(new { message = "Car is no longer available" });
                }

                // Check for conflicting PAID bookings
                var conflictingBooking = await _context.TrRentals
                    .Where(r => r.Car_id == rental.Car_id &&
                                r.Rental_id != rental.Rental_id &&
                                r.Payment_status == true &&
                                ((rental.Rental_date >= r.Rental_date && rental.Rental_date < r.Return_date) ||
                                 (rental.Return_date > r.Rental_date && rental.Return_date <= r.Return_date) ||
                                 (rental.Rental_date <= r.Rental_date && rental.Return_date >= r.Return_date)))
                    .AnyAsync();

                if (conflictingBooking)
                {
                    return BadRequest(new { message = "Car is no longer available for these dates" });
                }

                // Update rental to PAID
                rental.Payment_status = true;

                // Create payment record
                var payment = new LtPayment
                {
                    Payment_id = Guid.NewGuid().ToString(),
                    Rental_id = rental.Rental_id,
                    Payment_date = DateTime.Now,
                    Amount = rental.Total_price,
                    Payment_method = dto.Payment_method
                };

                await _context.LtPayments.AddAsync(payment);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new CheckoutResponseDto
                {
                    Rental_id = rental.Rental_id,
                    Payment_id = payment.Payment_id,
                    Total_price = rental.Total_price,
                    Message = "Payment successful!"
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "Error processing payment", error = ex.Message });
            }
        }
    }
}
