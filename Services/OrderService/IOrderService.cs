using LittleLemon_API.Models;

namespace LittleLemon_API.Services.OrderService;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order> GetOrderByIdAsync(int orderId);
    Task<Order> CreateOrderAsync(Order order);
    Task<Order> UpdateOrderAsync(Order order);
    Task DeleteOrderAsync(int orderId);
}