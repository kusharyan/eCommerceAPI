using eCommerceApi.Data;
using eCommerceApi.Exceptions;
using eCommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApi.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(AppDbContext context, ILogger<CustomerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Customer> Add(Customer customer)
        {
            _logger.LogInformation("New Customer Added Name: {name}, Email: {email}", customer.Name, customer.Email);
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            _logger.LogInformation("Getting all User details");
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            _logger.LogInformation("Getting details of User ID: {userId}", id);
            var user = await _context.Customers.FindAsync(id);

            if(user == null)
            {
                throw new NotFoundException($"Customer with ID: {id} not found!");
            }
            
            return await _context.Customers.FindAsync(id);
        }
    }
}