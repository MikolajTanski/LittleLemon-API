﻿using System.Reflection;
using LittleLemon_API.Models;
using LittleLemon_API.Services.OrderService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LittleLemon_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    //refleksja -  bez wiekszego sensu ale jest mtan
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrdersWithReflection()
    {
        // Typ
        Type serviceType = _orderService.GetType();
        // Metoda do pobrania zamówień xd
        MethodInfo methodInfo = serviceType.GetMethod("GetOrdersAsync");
        if (methodInfo != null)
        {
            // Dynamiczne wywołanie metody xdd
            var orders = await (Task<IEnumerable<Order>>)methodInfo.Invoke(_orderService, null);
            return Ok(orders);
        }

        return NotFound();
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<Order>> PostOrder(Order order)
    {
        var createdOrder = await _orderService.CreateOrderAsync(order);
        return CreatedAtAction("GetOrder", new { id = createdOrder.Id }, createdOrder);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutOrder(int id, Order order)
    {
        if (id != order.Id)
        {
            return BadRequest();
        }

        try
        {
            await _orderService.UpdateOrderAsync(order);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await OrderExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        try
        {
            await _orderService.DeleteOrderAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    private async Task<bool> OrderExists(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        return order != null;
    }
}
