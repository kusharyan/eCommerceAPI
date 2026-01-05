using eCommerceApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApi.Repository
{
    [ApiController]
    [Route("api/Orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersRepository _ordersRepo;
        private readonly ICartRepository _cartRepo;
        private readonly IProductsRepository _productsRepo;

        public OrdersController(IOrdersRepository ordersRepo, ICartRepository cartRepo, IProductsRepository productsRepo)
        {
            _ordersRepo = ordersRepo;
            _cartRepo = cartRepo;
            _productsRepo = productsRepo;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrder(int userId)
        {
            var cartItems = await _cartRepo.GetUserCartAsync(userId);

            if(!cartItems.Any())
            {
                return BadRequest("Cart is Empty");
            }

            var order = new Order
            {
                CustomerId = userId,
                OrderDate = DateTime.UtcNow,
                Items = new List<OrderItem>()
            };

            foreach (var item in cartItems)
            {
                var product = await _productsRepo.GetByIdAsync(item.ProductId);

                if (product.Stock < item.Quantity)
                    return BadRequest($"Not enough stock for {product.Name}");

                product.Stock -= item.Quantity;
                await _productsRepo.UpdateAsync(product);

                order.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            await _ordersRepo.CreateOrderAsync(order);
            await _cartRepo.ClearCartAsync(userId);

            return Ok(order);
        }
    }
}