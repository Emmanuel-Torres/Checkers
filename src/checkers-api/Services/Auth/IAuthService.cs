using checkers_api.Models.ExternalModels;
using checkers_api.Models.PrimitiveModels;

namespace checkers_api.Services;

public interface IAuthService
{
    Task<bool> ValidateTokenAsync(string token);
    Task<Profile?> GetUserAsync(string token);
    Task UpdateProfileAsync(string token, ProfileUpdateRequest request);
    Task Logout(string token);
}