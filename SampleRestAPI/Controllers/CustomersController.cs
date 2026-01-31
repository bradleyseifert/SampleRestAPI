using Microsoft.AspNetCore.Mvc;
using SampleRestAPI.Interfaces;
using SampleRestAPI.Models;

namespace SampleRestAPI.Controllers;

/// <summary>
/// Represents an API controller that provides endpoints for managing customer records.
/// </summary>
/// <remarks>The CustomersController exposes RESTful endpoints for creating, retrieving, updating, and deleting
/// customers. All routes are prefixed with 'api/Customers'. This controller is intended to be used as part of an
/// ASP.NET Core Web API and relies on dependency injection for accessing customer data through the ICustomerRepository
/// interface.</remarks>
[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _repo;

    public CustomersController(ICustomerRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Retrieves a list of all customers.
    /// </summary>
    /// <returns>An asynchronous operation that returns the list of all customers. Collection is empty if no customers are found.</returns>
    // Request: GET: api/Customers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(CancellationToken cancellationToken)
    {
        var items = await _repo.GetCustomersAsync(cancellationToken);
        return Ok(items);
    }

    /// <summary>
    /// Retrieves the customer with the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the customer to retrieve.</param>
    /// <returns>The customer if found; otherwise, a 404 Not Found response.</returns>
    // Request: GET: api/Customers/
    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetById(long id)
    {
        var item = await _repo.GetCustomerByIdAsync(id);
        if (item == null)
            return NotFound();
        return Ok(item);
    }

    /// <summary>
    /// Update the customer with the specified unique identifier.
    /// </summary>
    /// <param name="id">CustomerId of the desired customer to update</param>
    /// <param name="customerInput">The customer structure with the desired data</param>
    /// <returns>400 http bad request if the CustomerId doesn't match the data structure
    /// 404 http not found if the Customer doesn't exist
    /// 200 with the stored Customer structure upon success</returns>
    // Request: PUT: api/Customers/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Customer>> Update(long id, Customer customerInput)
    {
        //Check for data match
        if (id != customerInput.CustomerId)
        {
            return BadRequest();
        }

        //execute the update
        var customerResult = await _repo.UpdateCustomerAsync(id, customerInput);

        //if the customer was not found, return 404
        if (customerResult == null)
        {
            return NotFound();
        }

        //return the updated customer
        return Ok(customerResult);
    }

    /// <summary>
    /// Creates a new customer and returns the created customer resource.
    /// </summary>
    /// <remarks>If the creation is successful, the response includes a 201 Created status code and the
    /// details of the newly created customer. The location header in the response points to the endpoint for retrieving
    /// the created customer by ID.</remarks>
    /// <param name="customerInput">The customer information to create. Must not be null.</param>
    /// <returns>The created customer and a location header with the URI of the new resource.</returns>
    // Request: POST: api/Customers
    [HttpPost]
    public async Task<ActionResult<Customer>> Create(Customer customerInput)
    {
        var created = await _repo.AddCustomerAsync(customerInput);
        return CreatedAtAction(nameof(GetById), new { id = created?.CustomerId }, created);
    }

    /// <summary>
    /// Deletes the customer with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the customer to delete.</param>
    /// <returns>A response indicating the result of the delete operation. Returns a 204 No Content response if the customer was
    /// successfully deleted.</returns>
    // DELETE: api/Customers/1
    [HttpDelete("{id}")]
    public async Task<ActionResult<Customer>> Delete(long id)
    {
        var deleted = await _repo.DeleteCustomerAsync(id);
        return NoContent();
    }
}