using eCommerceApi.Models;
using eCommerceApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceApi.Controllers
{
    [ApiController]
    [Route("/api/Customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _repository;

        public CustomersController(ICustomerRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            await _repository.Add(customer);
            return Ok(customer);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            return Ok(await _repository.GetAllUsers());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var user = await _repository.GetUserByIdAsync(id);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }
    }
}