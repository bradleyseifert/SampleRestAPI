using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleRestAPI.Models;

namespace SampleRestAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly SampleRestAPIContext _context;

    public CustomersController(SampleRestAPIContext context)
    {
        _context = context;
    }

    // GET: api/Customers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomersTDO>>> GetCustomers()
    {
        return await _context.Customers
            .Include(c => c.Orders)
            .Select(x => CustomerToDTO(x))
            .ToListAsync();
    }

    // GET: api/Customers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomersTDO>> GetCustomer(long id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
        {
            return NotFound();
        }

        return CustomerToDTO(customer);
    }

    // PUT: api/Customers/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCustomer(long id, CustomersTDO customerTDO)
    {
        if (id != customerTDO.CustomerId)
        {
            return BadRequest();
        }

        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            return NotFound();
        }

        customer.Name = customerTDO.Name;
        customer.Orders = customerTDO.Orders;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!CustomerExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/Customers
    [HttpPost]
    public async Task<ActionResult<CustomersTDO>> PostCustomer(CustomersTDO customerTDO)
    {
        var customer = new Customer
        {
            Name = customerTDO.Name,
            Orders = new List<Order>()
        };

        if (customer.Orders != null)
        {
            foreach (var order in customerTDO?.Orders)
            {
                customer.Orders.Add(order);
                //_context.Orders.Add(new Order { OrderNumber = order.OrderNumber, ProductName = order.ProductName});
            }

            //await _context.SaveChangesAsync();
        }

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCustomer),
            new { id = customer.CustomerId },
            CustomerToDTO(customer));
    }

    // DELETE: api/Customers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(long id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            return NotFound();
        }

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CustomerExists(long id)
    {
        return _context.Customers.Any(e => e.CustomerId == id);
    }

    private static CustomersTDO CustomerToDTO(Customer customer) =>
       new CustomersTDO
       {
           CustomerId = customer.CustomerId,
           Name = customer.Name,
           Orders = customer.Orders
       };
}