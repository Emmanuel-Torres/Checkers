using checkers_api.Models.PersistentModels;

namespace checkers_api.Services;

public interface IUserDbService
{
    Task<DbProfile?> GetUserByEmailAsync(string userEmail);
    Task<DbProfile?> GetUserByIdAsync(string userId);
    Task AddUserAsync(DbProfile user);
    Task RemoveUserByIdAsync(int userId);
    Task UpdateUserAsync(DbProfile user);
}