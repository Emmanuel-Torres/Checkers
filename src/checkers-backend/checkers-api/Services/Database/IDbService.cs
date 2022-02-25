using checkers_api.Models;

namespace checkers_api.Services;

public interface IDbService
{
    Task<UserProfile?> GetUserByEmailAsync(string userEmail);
    Task<UserProfile?> GetUserByIdAsync(int userId);
    Task<UserProfile> AddUserAsync(UserProfile user);
    Task<UserProfile> RemoveUserByIdAsync(int userId);
    // Task<User> RemoveUserByEmailAsync(string userEmail);
    Task<UserProfile> UpdateUserAsync(int userId, UserProfile user);
}