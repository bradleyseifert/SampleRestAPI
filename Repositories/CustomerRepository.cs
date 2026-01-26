using Microsoft.EntityFrameworkCore;
using SampleRestAPI.Interfaces;
using SampleRestAPI.Models;

namespace SampleRestAPI.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SampleRestAPIContext _context;

        public CustomerRepository(SampleRestAPIContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await _context.Customers
                .Include(c => c.Orders)
                .Select(x => CustomerToDTO(x))
                .ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(long id)
        {
            return await _context.Customers.Include(c => c.Orders).AsNoTracking().FirstOrDefaultAsync(o => o.CustomerId == id);
        }

        public async Task<Customer> UpdateCustomerAsync(long id, Customer customerInput)
        {
            try
            {
                if (CustomerExists(id))
                {
                    var customerResult = await _context.Customers.FindAsync(id);

                    //Update values based on input
                    customerResult.Name = customerInput.Name;
                    customerResult.Orders = customerInput.Orders;

                    await _context.SaveChangesAsync();

                    return customerResult;
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

        public async Task<Customer> AddCustomerAsync(Customer customerInput)
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

            return customer;
        }

        public async Task<Customer> DeleteCustomerAsync(long id)
        {
            try
            {
                if (CustomerExists(id))
                {
                    var customer = await _context.Customers.FindAsync(id);

                    _context.Customers.Remove(customer);
                    await _context.SaveChangesAsync();

                    return customer;
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
}
