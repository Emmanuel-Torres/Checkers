using checkers_api.Models;

namespace checkers_api.Services;

public interface IDbService
{
    Task<User?> GetUserByEmailAsync(string userEmail);
    Task<User?> GetUserByIdAsync(int userId);
    Task<User> AddUserAsync(User user);
    Task<User> RemoveUserByIdAsync(int userId);
    // Task<User> RemoveUserByEmailAsync(string userEmail);
    Task<User> UpdateUserAsync(int userId, User user);
}