using eCommerceApi.Models;

namespace eCommerceApi.Repository
{
    public interface ICartRepository
    {
        Task AddToCartAsync(CartItem item);
        Task<List<CartItem>> GetUserCartAsync(int userId);
        Task ClearCartAsync(int userId);
    }
}