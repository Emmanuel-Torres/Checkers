using checkers_api.Models.ExternalModels;
using checkers_api.Models.PersistentModels;

namespace checkers_api.Services;

public interface IAuthService
{
    Task<bool> ValidateTokenAsync(string token);
    Task<UserProfile?> GetUserAsync(string token);
    Task UpdateProfileAsync(string token, ProfileUpdateRequest request);
    Task Logout(string token);
}