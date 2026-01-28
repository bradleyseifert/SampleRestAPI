using Microsoft.EntityFrameworkCore;
using SampleRestAPI.Interfaces;
using SampleRestAPI.Models;

namespace SampleRestAPI.Repositories
{
    /// <summary>
    /// Provides methods for managing customer entities in the data store, including retrieval, creation, updating, and
    /// deletion operations.
    /// </summary>
    /// <remarks>This repository encapsulates data access logic for customers and their related orders,
    /// enabling operations against the underlying database. Methods in this class are intended to
    /// be used by application services or controllers to interact with customer data without exposing database
    /// implementation details. All operations are performed asynchronously to support scalable application
    /// patterns.</remarks>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SampleRestAPIContext _context;

        public CustomerRepository(SampleRestAPIContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Asynchronously retrieves a list of customers, including their associated orders.
        /// </summary>
        /// <returns>A list of customers with their orders included. The list is empty if no customers are found.</returns>
        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await _context.Customers
                .Include(c => c.Orders)
                .Select(x => CustomerToDTO(x))
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier, including their associated orders.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to retrieve.</param>
        /// <returns>The customer with the specified identifier and their orders, or null if no matching customer is found.</returns>
        public async Task<Customer?> GetCustomerByIdAsync(long id)
        {
            return await _context.Customers.Include(c => c.Orders).AsNoTracking().FirstOrDefaultAsync(o => o.CustomerId == id);
        }

        /// <summary>
        /// Updates the details of an existing customer with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to update.</param>
        /// <param name="customerInput">An object containing the updated customer information. The properties of this object will overwrite the
        /// corresponding properties of the existing customer.</param>
        /// <returns>The updated customer if the customer exists; otherwise, null.</returns>
        public async Task<Customer?> UpdateCustomerAsync(long id, Customer customerInput)
        {
            try
            {
                if (CustomerExists(id))
                {
                    //fetch the customer to update
                    var customerResult = await _context.Customers.FindAsync(id);

                    if (customerResult == null)
                    {
                        return null;
                    }
                    else
                    {
                        //Update values based on input
                        customerResult.Name = customerInput.Name;
                        customerResult.Orders = customerInput.Orders;
                        await _context.SaveChangesAsync();

                        return customerResult;
                    }
                    
                }
                else
                {
                    return null;
                }
            }
            // Handle concurrency issues where the customer might have been deleted by another process
            catch (DbUpdateConcurrencyException) when (!CustomerExists(id))
            {
                return null;
            }
        }

        /// <summary>
        /// Adds a new customer to the data store based on the provided customer information.
        /// </summary>
        /// <param name="customerInput">The customer data to add. The Name property must be set. If the Customer.Orders collection is provided,
        /// its orders are associated with the new customer. Cannot be null.</param>
        /// <returns>The newly added Customer, including any generated identifiers.</returns>
        public async Task<Customer> AddCustomerAsync(Customer customerInput)
        {
            var customer = new Customer
            {
                Name = customerInput.Name,
                Orders = new List<Order>()
            };

            // Add orders if provided
            if (customer.Orders != null)
            {
                customer.Orders.AddRange(from order in customerInput?.Orders
                                         select order);
            }

            // Add the new customer to the context and save changes
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        /// <summary>
        /// Deletes the customer with the specified identifier.
        /// </summary>
        /// <remarks>If the customer does not exist, the method returns null and no action is taken.
        /// This method performs the deletion and saves changes to the underlying data store.</remarks>
        /// <param name="id">The unique identifier of the customer to delete.</param>
        /// <returns>The deleted Customer if the customer existed; otherwise, null.</returns>
        public async Task<Customer?> DeleteCustomerAsync(long id)
        {
            try
            {
                if (CustomerExists(id))
                {
                    //fetch the customer to delete
                    var customer = await _context.Customers.FindAsync(id);

                    if (customer == null)
                    {
                        return null;
                    }
                    else
                    {
                        // Remove the customer from the context and save changes
                        _context.Customers.Remove(customer);
                        await _context.SaveChangesAsync();

                        return customer;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (DbUpdateConcurrencyException) when (!CustomerExists(id))
            {
                return null;
            }
        }

        /// <summary>
        /// Determines whether a customer with the specified identifier exists in the data store.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to locate.</param>
        /// <returns>true if a customer with the specified identifier exists; otherwise, false.</returns>
        private bool CustomerExists(long id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }

        /// <summary>
        /// Creates a new Customer object that is a data transfer representation of the specified customer.
        /// </summary>
        /// <param name="customer">The customer to convert to a data transfer object. Cannot be null.</param>
        /// <returns>A new Customer object containing the CustomerId, Name, and Orders from the specified customer.</returns>
        private static Customer CustomerToDTO(Customer customer) =>
           new Customer
           {
               CustomerId = customer.CustomerId,
               Name = customer.Name,
               Orders = customer.Orders
           };
    }
}
