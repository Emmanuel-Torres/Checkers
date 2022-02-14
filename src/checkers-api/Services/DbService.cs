using checkers_api.Data;
using checkers_api.Models;
using Microsoft.EntityFrameworkCore;

namespace checkers_api.Services;

public class DbService : IDbServce
{
    private readonly ApplicationDbContext dbContext;

    public DbService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<User> AddUserAsync(User user)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return await dbContext.Users.FirstAsync(u => u.Id == user.Id);
    }

    public async Task<User?> GetUserByEmailAsync(string userEmail)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User> RemoveUserAsync(int userId)
    {
        User user = await dbContext.Users.FirstAsync(u => u.Id == userId);
        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUserAsync(int userId, User user)
    {
        user.Id = userId;
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();
        return await dbContext.Users.FirstAsync(u => u.Id == userId);
    }
}