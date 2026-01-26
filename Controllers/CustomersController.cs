using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleRestAPI.Models;
using SampleRestAPI.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using SampleRestAPI.Wrappers;
using System.Linq;

namespace SampleRestAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ICustomerRepository
{
    private readonly SampleRestAPIContext _context;

    public CustomersController(SampleRestAPIContext context)
    {
        _context = context;
    }

    // GET: api/Customers
    [HttpGet]
    public async Task<List<Customer>> GetCustomersAsync()
    {
        return await _context.Customers
            .Include(c => c.Orders)
            .Select(x => CustomerToDTO(x))
            .ToListAsync();
    }

    // GET: api/Customers/5
    [HttpGet("{id}")]
    public async Task<CustomerResponse> GetCustomerByIdAsync(long id)
    {
        var customer = await _context.Customers.Include(c => c.Orders).FirstOrDefaultAsync(o => o.CustomerId == id);

        if (customer == null)
        {
            return new CustomerResponse { Message = "Not found" };
        }

        return new CustomerResponse{ CustomerData = CustomerToDTO(customer) };
    }

    // PUT: api/Customers/5
    [HttpPut("{id}")]
    public async Task<CustomerResponse> UpdateCustomerAsync(long id, Customer customerInput)
    {
        if (id != customerInput.CustomerId)
        {
            return new CustomerResponse { Message = "Bad Request" };
        }

        var customerResult = await _context.Customers.FindAsync(id);
        if (customerResult == null)
        {
            return new CustomerResponse { Message = "Not found" };
        }

        //Update values based on input
        customerResult.Name = customerInput.Name;
        customerResult.Orders = customerInput.Orders;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!CustomerExists(id))
        {
            return new CustomerResponse { Message = "Not found" };
        }

        return new CustomerResponse { CustomerData = customerResult, Message = "Success" };
    }

    // POST: api/Customers
    [HttpPost]
    public async Task<CustomerResponse> AddCustomerAsync(Customer customerInput)
    {
        var customer = new Customer
        {
            Name = customerInput.Name,
            Orders = new List<Order>()
        };

        if (customer.Orders != null)
        {
            customer.Orders.AddRange(from order in customerInput?.Orders
                                     select order);
        }

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return new CustomerResponse { CustomerData = customer, Message = "Success" };

        /*
        return CreatedAtAction(
            nameof(GetCustomer),
            new { id = customer.CustomerId },
            CustomerToDTO(customer));
        */
    }

    // DELETE: api/Customers/5
    [HttpDelete("{id}")]
    public async Task<CustomerResponse> DeleteCustomerAsync(long id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            return new CustomerResponse { Message = "Not found" };
        }

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();

        return new CustomerResponse { CustomerData = customer, Message = "Success" };
    }

    private bool CustomerExists(long id)
    {
        return _context.Customers.Any(e => e.CustomerId == id);
    }

    private static Customer CustomerToDTO(Customer customer) =>
       new Customer
       {
           CustomerId = customer.CustomerId,
           Name = customer.Name,
           Orders = customer.Orders
       };
}