using LittleLemon_API.Data;
using LittleLemon_API.Models;
using Microsoft.EntityFrameworkCore;

namespace LittleLemon_API.Repository.OrderRepository;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        return await _context.Orders.Include(o => o.Meals).ToListAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int orderId)
    {
        var result = await _context.Orders
            .Include(o => o.Meals)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        return result;
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> UpdateOrderAsync(Order order)
    {
        _context.Entry(order).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task DeleteOrderAsync(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}