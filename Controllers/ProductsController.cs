using eCommerceApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApi.Controllers
{
    [ApiController]
    [Route("api/Products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository _repository;

        public ProductsController(IProductsRepository repository)
        {   
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(await _repository.GetAllAsync());
        }
    }
}