using eCommerceApi.Exceptions;
using eCommerceApi.Models;
using eCommerceApi.Repository;

namespace eCommerceApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductsRepository _productRepository;

        public OrderService(ICartRepository cartRepository, IProductsRepository productRepository, IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<Order> CreateOrder(int userId)
        {
            var cartItems = await _cartRepository.GetUserCartAsync(userId);

            if(!cartItems.Any())
            {
                throw new ConflictException("Cart is Empty");
            }

            var order = new Order
            {
                CustomerId = userId,
                OrderDate = DateTime.UtcNow,
                Items = new List<OrderItem>()
            };

            foreach (var item in cartItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);

                if (product.Stock < item.Quantity)
                    throw new ConflictException($"Not enough stock for {product.Name}");

                product.Stock -= item.Quantity;
                await _productRepository.UpdateAsync(product);

                order.Items.Add(new OrderItem
                {
                    CustomerId = userId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            await _ordersRepository.CreateOrderAsync(order);
            await _cartRepository.ClearCartAsync(userId);

            return order;
        }
    }
}