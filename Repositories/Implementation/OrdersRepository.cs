using eCommerceApi.Data;
using eCommerceApi.Models;

namespace eCommerceApi.Repository
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly AppDbContext _context;

        public OrdersRepository(AppDbContext context)
        {
            _context = context; 
        }

        public async Task CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
    }
}