using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.API.Data;
using RentCar.API.DTOs.Cars;


namespace RentCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cars?rentDate={rentDate}&returnDate={returnDate}&page={page}&sortBy={sortBy}&sortOrder={sortOrder}
        [HttpGet]
        public async Task<ActionResult<CarAPIResponse>> GetAvailableCars(
            [FromQuery] DateTime? rentDate,
            [FromQuery] DateTime? returnDate,
            [FromQuery] int? yearFilter,
            [FromQuery] int page = 1,
            [FromQuery] string sortBy = "Name",
            [FromQuery] string sortOrder = "asc")
        {
            const int pageSize = 3;

            // Return no cars if rent dates are invalid
            if (!rentDate.HasValue || !returnDate.HasValue || rentDate >= returnDate)
            {
                return Ok(new CarAPIResponse
                {
                    Cars = new List<CarDto>(),
                    CurrentPage = page,
                    TotalPages = 0,
                    TotalCars = 0,
                    PageSize = pageSize
                });
            }

            try
            {


                // Get available cars within a range of dates with their images
                var query = _context.MsCars
                    .Include(c => c.CarImages)
                    .Include(c => c.Rentals)
                    .Where(c => c.Status == true)
                    .Where(c =>
                        !c.Rentals.Any(r =>
                            rentDate.Value < r.Return_date &&
                            returnDate.Value > r.Rental_date
                        )
                    );

                // Apply year filter if provided
                if (yearFilter.HasValue)
                {
                    query = query.Where(c => c.Year == yearFilter.Value);
                }

                // Apply sorting
                query = sortBy switch
                {
                    "Name" => sortOrder == "asc" ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name),
                    "Model" => sortOrder == "asc" ? query.OrderBy(c => c.Model) : query.OrderByDescending(c => c.Model),
                    "Year" => sortOrder == "asc" ? query.OrderBy(c => c.Year) : query.OrderByDescending(c => c.Year),
                    "Price" => sortOrder == "asc" ? query.OrderBy(c => c.Price_per_day) : query.OrderByDescending(c => c.Price_per_day),
                    _ => query.OrderBy(c => c.Name)
                };

                // Get total count
                var totalCars = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalCars / (double)pageSize);

                // Ensure page is within valid range
                page = Math.Max(1, Math.Min(page, Math.Max(totalPages, 1)));

                // Get paginated results
                var cars = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new CarDto
                    {
                        Car_id = c.Car_id,
                        Name = c.Name,
                        Model = c.Model,
                        Year = c.Year,
                        License_plate = c.License_plate,
                        Number_of_car_seats = c.Number_of_car_seats,
                        Transmission = c.Transmission,
                        Price_per_day = c.Price_per_day,
                        Status = c.Status,
                        CarImages = c.CarImages.Select(img => new CarImageDto
                        {
                            Image_car_id = img.Image_car_id,
                            Car_id = img.Car_id,
                            Image_link = img.Image_link
                        }).ToList()
                    })
                    .ToListAsync();

                var response = new CarAPIResponse
                {
                    Cars = cars,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalCars = totalCars,
                    PageSize = pageSize
                };

                Console.WriteLine("response: ", response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching cars.", error = ex.Message });
            }
        }

        // GET: api/Car/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CarDto>> GetCar(string id)
        {
            var car = await _context.MsCars
                .Include(c => c.CarImages)
                .FirstOrDefaultAsync(c => c.Car_id == id);

            if (car == null)
            {
                return NotFound();
            }

            var carDto = new CarDto
            {
                Car_id = car.Car_id,
                Name = car.Name,
                Model = car.Model,
                Year = car.Year,
                License_plate = car.License_plate,
                Number_of_car_seats = car.Number_of_car_seats,
                Transmission = car.Transmission,
                Price_per_day = car.Price_per_day,
                Status = car.Status,
                CarImages = car.CarImages.Select(img => new CarImageDto
                {
                    Image_car_id = img.Image_car_id,
                    Car_id = img.Car_id,
                    Image_link = img.Image_link
                }).ToList()
            };

            return Ok(carDto);
        }
    }

    public class CarAPIResponse
    {
        public List<CarDto> Cars { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCars { get; set; }
        public int PageSize { get; set; }
    }
}
