using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations;

public class UserService: IUserService
{
    private readonly ApplicationDbContext context;

    public UserService(ApplicationDbContext context)
    {
        this.context = context;
    }
    
    public async Task<User> GetUser()
    {
        return await context.Users
            .Include(user => user.Orders)
            .OrderByDescending(user =>user.Orders
                .Where(order => order.Status == OrderStatus.Delivered && order.CreatedAt.Year == 2003)
                .Sum(order => order.Quantity*order.Price))
            .FirstAsync();
    }

    public async Task<List<User>> GetUsers()
    {
        return await context.Users.
            Include(user => user.Orders)
            .Where(user => user.Orders.Count(
                    order =>  order.CreatedAt.Year == 2010  &&
                    order.Status == OrderStatus.Paid
                ) > 0
            ).ToListAsync();
    }
}