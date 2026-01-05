using eCommerceApi.Models;

namespace eCommerceApi.Repository
{
    public interface IProductsRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task UpdateAsync(Product product);
    }
}