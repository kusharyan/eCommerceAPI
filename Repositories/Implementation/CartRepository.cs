using eCommerceApi.Data;
using eCommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApi.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddToCartAsync (CartItem item)
        {
            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CartItem>> GetUserCartAsync(int userId)
        {
            return await _context.CartItems.Where(u => u.CustomerId == userId).ToListAsync();
        }

        public async Task ClearCartAsync(int userId)
        {
            var items = _context.CartItems.Where(items => items.CustomerId == userId);
            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}