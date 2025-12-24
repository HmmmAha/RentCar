using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RentCar.API.Models;
using RentCar.API.Data;

namespace RentCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MsCustomer>>> GetCustomers()
        {
            return await _context.MsCustomers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MsCustomer>> GetCustomer(string id)
        {
            var customer = await _context.MsCustomers
                .Include(c => c.Rentals)
                .FirstOrDefaultAsync(c => c.Customer_id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<MsCustomer>> PostCustomer(MsCustomer customer)
        {
            _context.MsCustomers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Customer_id }, customer);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(string id, MsCustomer customer)
        {
            if (id != customer.Customer_id)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            var customer = await _context.MsCustomers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.MsCustomers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(string id)
        {
            return _context.MsCustomers.Any(e => e.Customer_id == id);
        }
    }
}
