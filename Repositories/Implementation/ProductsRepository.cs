using eCommerceApi.Data;
using eCommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApi.Repository
{
    public class ProductRepository : IProductsRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(AppDbContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            _logger.LogInformation("Fetched all Products!");
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching products by ID: {id}", id);
            return await _context.Products.FindAsync(id);
        }

        public async Task UpdateAsync(Product product)
        {
            _logger.LogInformation("Updated Product name: {Name}, Pirce: {Price}, Stock: {Stock} ", product.Name, product.Price, product.Stock);
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}