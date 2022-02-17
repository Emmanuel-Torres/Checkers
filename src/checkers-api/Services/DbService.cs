using checkers_api.Data;
using checkers_api.Models;
using Microsoft.EntityFrameworkCore;

namespace checkers_api.Services;

public class DbService : IDbService
{
    private readonly ApplicationDbContext dbContext;
    private readonly ILogger<DbService> logger;

    public DbService(ApplicationDbContext dbContext, ILogger<DbService> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<User> AddUserAsync(User user)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return await dbContext.Users.FirstAsync(u => u.Id == user.Id);
    }

    public async Task<User?> GetUserByEmailAsync(string userEmail)
    {
        try
        {
            logger.LogDebug("[{location}]: Retrieving user profile for {email}", nameof(DbService), userEmail);
            var user = await dbContext.Users.FirstAsync(u => u.Email == userEmail);
            logger.LogDebug("[{location}]: User profile found for {email}", nameof(DbService), userEmail);
            return user;
        }
        catch (ArgumentNullException ex)
        {
            logger.LogWarning("[{location}]: User profile does not exist for {email}. Ex: {ex}", nameof(DbService), userEmail, ex);
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not retrieve user profile for {email}. Ex: {ex}", nameof(DbService), userEmail, ex);
        }
        return null;
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