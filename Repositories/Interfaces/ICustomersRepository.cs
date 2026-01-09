using eCommerceApi.Models;

namespace eCommerceApi.Repository
{
    public interface ICustomerRepository
    {
        Task<Customer> Add(Customer customer);
        Task<List<Customer>> GetAllCustomers();
        Task<Customer?> GetCustomerByIdAsync(int id);
    }
}