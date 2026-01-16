using eCommerceApi.Models;

namespace eCommerceApi.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(int userId);
    }
}