using checkers_api.Models.PrimitiveModels;

namespace checkers_api.Services;

public interface IDbService
{
    public Task<Profile?> GetUserByEmailAsync(string userEmail);
    public Task<Profile?> GetUserByIdAsync(string userId);
    public Task AddUserAsync(Profile user);
    public Task RemoveUserByIdAsync(int userId);
    public Task UpdateUserAsync(Profile user);
}