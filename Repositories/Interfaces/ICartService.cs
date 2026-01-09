namespace eCommerceApi.Services
{
    public interface ICartService
    {
        Task AddToCart(int userId, int productId, int quantity);
    }
}