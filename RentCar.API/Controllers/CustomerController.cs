using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.API.Data;
using RentCar.API.Models;

namespace RentCar.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MsCustomer>>> GetCustomers()
        {
            return await _context.MsCustomers.ToListAsync();
        }

        // GET: api/Customer
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

        // POST: api/Customer
        [HttpPost]
        public async Task<ActionResult<MsCustomer>> PostCustomer(MsCustomer customer)
        {
            _context.MsCustomers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Customer_id }, customer);
        }

        // PUT: api/Customer/{id}
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

        // DELETE: api/Customer/{id}
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
