using Microsoft.AspNetCore.Mvc;
using SampleRestAPI.Interfaces;
using SampleRestAPI.Models;

namespace SampleRestAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _repo;
    public CustomersController(ICustomerRepository repo)
    {
        _repo = repo;
    }

    // GET: api/Customers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        var items = await _repo.GetCustomersAsync();
        return Ok(items);
    }

    // GET: api/Customers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetById(long id)
    {
        var item = await _repo.GetCustomerByIdAsync(id);
        if (item == null)
            return NotFound();
        return Ok(item);
    }

    // PUT: api/Customers/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Customer>> Update(long id, Customer customerInput)
    {
        if (id != customerInput.CustomerId)
        {
            return BadRequest();
        }

        var customerResult = await _repo.UpdateCustomerAsync(id, customerInput);

        if (customerResult == null)
        {
            return NotFound();
        }

        return Ok(customerResult);
    }

    // POST: api/Customers
    [HttpPost]
    public async Task<ActionResult<Customer>> Create(Customer customerInput)
    {
        var created = await _repo.AddCustomerAsync(customerInput);
        return CreatedAtAction(nameof(GetById), new { id = created?.CustomerId }, created);
    }

    // DELETE: api/Customers/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Customer>> Delete(long id)
    {
        var deleted = await _repo.DeleteCustomerAsync(id);
        return NoContent();
    }
}