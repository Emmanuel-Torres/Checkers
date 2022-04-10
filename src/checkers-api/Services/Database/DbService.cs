using checkers_api.Data;
using checkers_api.Models.PersistentModels;
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

    public async Task<UserProfile> AddUserAsync(UserProfile user)
    {
        try
        {
            logger.LogDebug("[{location}]: Adding user {email} to database", nameof(DbService), user.Email);
            await dbContext.UserProfiles.AddAsync(user);
            await dbContext.SaveChangesAsync();
            logger.LogDebug("[{location}]: User {email} successfully added to the database", nameof(DbService), user.Email);
            return await dbContext.UserProfiles.FirstAsync(u => u.Id == user.Id);
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not add user {email} to database. Ex: {ex}", nameof(DbService), user.Email, ex);
            throw;
        }
    }

    public async Task<UserProfile?> GetUserByEmailAsync(string userEmail)
    {
        try
        {
            logger.LogDebug("[{location}]: Retrieving user profile for {email}", nameof(DbService), userEmail);
            var user = await dbContext.UserProfiles.AsNoTracking().FirstAsync(u => u.Email == userEmail);
            logger.LogDebug("[{location}]: User profile found for {email}", nameof(DbService), userEmail);
            return user;
        }
        catch (ArgumentNullException ex)
        {
            logger.LogWarning("[{location}]: User profile {email} does not exist. Ex: {ex}", nameof(DbService), userEmail, ex);
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not retrieve user profile for {email}. Ex: {ex}", nameof(DbService), userEmail, ex);
        }
        return null;
    }

    public async Task<UserProfile?> GetUserByIdAsync(int userId)
    {
        try
        {
            logger.LogDebug("[{location}]: Retrieving user profile for id {id}", nameof(DbService), userId);
            var user = await dbContext.UserProfiles.FirstAsync(u => u.Id == userId);
            logger.LogDebug("[{location}]: User profile found for id {id}", nameof(DbService), userId);
            return user;
        }
        catch (ArgumentNullException ex)
        {
            logger.LogWarning("[{location}]: User profile for id {id} does not exist. Ex: {ex}", nameof(DbService), userId, ex);
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not retrieve user profile for id {id}. Ex: {ex}", nameof(DbService), userId, ex);
        }
        return null;
    }

    public async Task<UserProfile> RemoveUserByIdAsync(int userId)
    {
        try
        {
            logger.LogDebug("[{location}]: Removing user with id {id} from database", nameof(DbService), userId);
            var user = await dbContext.UserProfiles.FirstAsync(u => u.Id == userId);
            dbContext.UserProfiles.Remove(user);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("[{location}]: User {id} was successfully removed from the database", nameof(DbService), userId);
            return user;
        }
        catch (ArgumentNullException ex)
        {
            logger.LogWarning("[{location}]: Could not remove user with id {id} because it does not exist. Ex: {ex}", nameof(DbService), userId, ex);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not remove user with id {id}. Ex: {ex}", nameof(DbService), userId, ex);
            throw;
        }
    }

    public async Task UpdateUserAsync(UserProfile user)
    {
        var userId = user.Id;

        try
        {
            logger.LogDebug("[{location}]: Updating user with id {id}.", nameof(DbService), userId);
            dbContext.UserProfiles.Update(user);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("[{location}]: User {id} was successfully updated", nameof(DbService), userId);
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not update user with id {id}. Ex: {ex}", nameof(DbService), userId, ex);
            throw;
        }
    }
}