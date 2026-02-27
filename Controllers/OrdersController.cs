using Microsoft.AspNetCore.Mvc;
using eCommerceApi.Services;

namespace eCommerceApi.Controllers
{
    [ApiController]
    [Route("api/Orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrder(int userId)
        {
            var order = await _orderService.CreateOrder(userId);
            return Ok(order);
        }
    }
}