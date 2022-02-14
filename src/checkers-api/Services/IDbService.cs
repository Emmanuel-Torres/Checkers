using checkers_api.Models;

namespace checkers_api.Services;

public interface IDbServce
{
    Task<User?> GetUserByEmailAsync(string userEmail);
    Task<User?> GetUserByIdAsync(int userId);
    Task<User> AddUserAsync(User user);
    Task<User> RemoveUserAsync(int userId);
    Task<User> UpdateUserAsync(int userId, User user);
}