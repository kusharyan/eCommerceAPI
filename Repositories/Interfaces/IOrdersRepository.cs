using eCommerceApi.Models;

namespace eCommerceApi.Repository
{
    public interface IOrdersRepository
    {
        Task CreateOrderAsync(Order order);
    }
}