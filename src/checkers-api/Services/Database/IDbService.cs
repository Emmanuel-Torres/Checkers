using checkers_api.Models.PersistentModels;

namespace checkers_api.Services;

public interface IDbService
{
    Task<UserProfile?> GetUserByEmailAsync(string userEmail);
    Task<UserProfile?> GetUserByIdAsync(int userId);
    Task<UserProfile> AddUserAsync(UserProfile user);
    Task<UserProfile> RemoveUserByIdAsync(int userId);
    // Task<User> RemoveUserByEmailAsync(string userEmail);
    Task UpdateUserAsync(UserProfile user);
}