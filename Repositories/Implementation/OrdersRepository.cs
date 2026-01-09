using eCommerceApi.Data;
using eCommerceApi.Models;

namespace eCommerceApi.Repository
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrdersRepository> _logger;

        public OrdersRepository(AppDbContext context, ILogger<OrdersRepository> logger)
        {
            _context = context; 
            _logger = logger;
        }

        public async Task CreateOrderAsync(Order order)
        {
            _logger.LogInformation("Created Ordered for UserID: {userId}, OrderId: {id}, Items: {items} ", order.CustomerId, order.Id, order.Items);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
    }
}