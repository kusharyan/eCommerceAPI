using eCommerceApi.Exceptions;
using eCommerceApi.Models;
using eCommerceApi.Repository;

namespace eCommerceApi.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductsRepository _productsRepository;

        public CartService(ICartRepository cartRepository, IProductsRepository productsRepository)
        {
            _cartRepository = cartRepository;
            _productsRepository = productsRepository;
        }

        public async Task AddToCart(int userId, int productId, int quantity)
        {
            var product = await _productsRepository.GetByIdAsync(productId);

            if(product == null)
                throw new NotFoundException("Product Not Found");

            if(product.Stock < quantity)
                throw new ConflictException("Insufficient Stock!");

            await _cartRepository.AddToCartAsync(new CartItem
            {
               CustomerId = userId,
               ProductId = productId,
               Quantity = quantity 
            });
        }
    }
}