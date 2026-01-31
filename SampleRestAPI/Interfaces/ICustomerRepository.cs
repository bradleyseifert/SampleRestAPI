using SampleRestAPI.Models;

namespace SampleRestAPI.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetCustomersAsync(CancellationToken cancellationToken = default);
        Task<Customer?> GetCustomerByIdAsync(long id);
        Task<Customer> AddCustomerAsync(Customer item);
        Task<Customer?> UpdateCustomerAsync(long id, Customer customerInput);
        Task<Customer?> DeleteCustomerAsync(long id);
    }
}
