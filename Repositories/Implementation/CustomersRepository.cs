using eCommerceApi.Data;
using eCommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApi.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> Add(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<List<Customer>> GetAllUsers()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetUserByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }
    }
}