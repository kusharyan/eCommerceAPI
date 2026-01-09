using eCommerceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApi.Controllers
{
    [ApiController]
    [Route("api/Cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddToCart(int userId, int productId, int quantity)
        {
            await  _cartService.AddToCart(userId, productId, quantity);
            return Ok("Product Added to Cart!");
        }
    }
}