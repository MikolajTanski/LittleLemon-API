using LittleLemon_API.Models;

namespace LittleLemon_API.Repository.OrderRepository;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order> GetOrderByIdAsync(int orderId);
    Task<Order> CreateOrderAsync(Order order);
    Task<Order> UpdateOrderAsync(Order order);
    Task DeleteOrderAsync(int orderId);
}