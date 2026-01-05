using eCommerceApi.Models;
using eCommerceApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApi.Controllers
{
    [ApiController]
    [Route("api/Cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepo;
        private readonly IProductsRepository _productRepo;

        public CartController(ICartRepository cartRepo, IProductsRepository productRepo)
        {
            _cartRepo = cartRepo;
            _productRepo = productRepo;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddToCart(int userId, int productId, int quantity)
        {
            var product = await _productRepo.GetByIdAsync(productId);

            if(product == null || product.Stock < quantity)
            {
                return BadRequest("Insufficient Stock!");
            }

            await _cartRepo.AddToCartAsync(new CartItem
            {
               UserId = userId,
               ProductId = productId,
               Quantity = quantity 
            });

            return Ok("Product Added to Cart!");
        }
    }
}