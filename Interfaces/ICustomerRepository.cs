using SampleRestAPI.Wrappers;
using SampleRestAPI.Models;

namespace SampleRestAPI.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetCustomersAsync();
        Task<CustomerResponse> GetCustomerByIdAsync(long id);
        Task<CustomerResponse> AddCustomerAsync(Customer item);
        Task<CustomerResponse> UpdateCustomerAsync(long id, Customer customerInput);
        Task<CustomerResponse> DeleteCustomerAsync(long id);
    }
}
