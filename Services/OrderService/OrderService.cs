using LittleLemon_API.Models;
using LittleLemon_API.Repository.OrderRepository;

namespace LittleLemon_API.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        return await _orderRepository.GetOrdersAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int orderId)
    {
        return await _orderRepository.GetOrderByIdAsync(orderId);
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        return await _orderRepository.CreateOrderAsync(order);
    }

    public async Task<Order> UpdateOrderAsync(Order order)
    {
        return await _orderRepository.UpdateOrderAsync(order);
    }

    public async Task DeleteOrderAsync(int orderId)
    {
        var orderExists = await _orderRepository.GetOrderByIdAsync(orderId);
        if (orderExists == null)
        {
            throw new KeyNotFoundException($"Order with id {orderId} not found.");
        }

        await _orderRepository.DeleteOrderAsync(orderId);
    }
}