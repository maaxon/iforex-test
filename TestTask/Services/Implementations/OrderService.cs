using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations;

public class OrderService: IOrderService
{
    private readonly ApplicationDbContext context;
    
    public OrderService(ApplicationDbContext context)
    {
        this.context = context;
    }
    public async Task<Order> GetOrder()
    {
        return await context.Orders
            .Where(order =>order.Quantity>1)
            .OrderByDescending(order => order.CreatedAt)
            .FirstAsync();
    }

    public async Task<List<Order>> GetOrders()
    {
        return await context.Orders
            .Include(order => order.User)
            .Where(order => order.User.Status == UserStatus.Active)
            .OrderByDescending( order => order.CreatedAt )
            .ToListAsync();
    }
}