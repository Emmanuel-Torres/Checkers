using checkers_api.Models;
using Google.Apis.Auth;

namespace checkers_api.Services;

public class AuthService : IAuthService
{
    private readonly IDbServce dbServce;
    private string clientId = "203576300472-3j2eeg1m35ahrg4ar8srm36ul8d504h5.apps.googleusercontent.com";

    public AuthService(IDbServce dbServce)
    {
        this.dbServce = dbServce;
    }

    public async Task<User> GetUserAsync(string token)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>() { clientId }
        };
        var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);

        return await getUserOrDefaultAsync(payload);
    }

    public Task<User> RegisterUserAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    private async Task<User> getUserOrDefaultAsync(GoogleJsonWebSignature.Payload payload)
    {
        User? user = await dbServce.GetUserByEmailAsync(payload.Email);
        if (user is not null)
        {
            return user;
        }

        user = new User(payload.Email, payload.GivenName, payload.FamilyName, payload.Picture);
        return await dbServce.AddUserAsync(user);
    }
}