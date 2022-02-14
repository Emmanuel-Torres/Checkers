using checkers_api.Models;

namespace checkers_api.Services;

public class AuthService : IAuthService
{
    public AuthService()
    {
        
    }

    public Task<User> GetUserAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task<User> RegisterUserAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        throw new NotImplementedException();
    }
}