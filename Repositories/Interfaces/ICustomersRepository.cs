using eCommerceApi.Models;

namespace eCommerceApi.Repository
{
    public interface ICustomerRepository
    {
        Task<Customer> Add(Customer customer);
        Task<List<Customer>> GetAllUsers();
        Task<Customer?> GetUserByIdAsync(int id);
    }
}