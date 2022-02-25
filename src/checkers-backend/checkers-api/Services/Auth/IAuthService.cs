using checkers_api.Models;

namespace checkers_api.Services;

public interface IAuthService
{
    Task<bool> ValidateTokenAsync(string token);
    Task<UserProfile> GetUserAsync(string token);
}