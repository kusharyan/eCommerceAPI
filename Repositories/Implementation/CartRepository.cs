using eCommerceApi.Data;
using eCommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApi.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CartRepository> _logger;

        public CartRepository(AppDbContext context, ILogger<CartRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddToCartAsync (CartItem item)
        {
            _logger.LogInformation("Item added to cart for UserID: {userId}, ProductID: {productId}, Quantity: {qunatity}", item.CustomerId, item.ProductId, item.Quantity);
            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CartItem>> GetUserCartAsync(int userId)
        {
            _logger.LogInformation("Fetching Cart for UserId: {userId}", userId);
            return await _context.CartItems.Where(u => u.CustomerId == userId).ToListAsync();
        }

        public async Task ClearCartAsync(int userId)
        {
            _logger.LogInformation("Cleared Cart for UserID: {userId}", userId);
            var items = _context.CartItems.Where(items => items.CustomerId == userId);
            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}