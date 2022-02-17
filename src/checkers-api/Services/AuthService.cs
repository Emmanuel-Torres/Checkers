using System.Reflection.Metadata.Ecma335;
using checkers_api.Models;
using Google.Apis.Auth;

namespace checkers_api.Services;

public class AuthService : IAuthService
{
    private readonly IDbService dbService;
    private readonly ILogger<AuthService> logger;
    private string clientId = "203576300472-3j2eeg1m35ahrg4ar8srm36ul8d504h5.apps.googleusercontent.com";

    public AuthService(IDbService dbService, ILogger<AuthService> logger)
    {
        this.dbService = dbService;
        this.logger = logger;
    }

    public async Task<User> GetUserAsync(string token)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>() { clientId }
        };

        logger.LogDebug("[{location}]: Validating user token", nameof(AuthService));
        var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
        logger.LogInformation("[{location}]: Token was validated successfully", nameof(AuthService));

        var user = await dbService.GetUserByEmailAsync(payload.Email);
        if (user is not null)
        {
            logger.LogDebug("[{location}]: Returning user profile for {email}", nameof(AuthService), payload.Email);
            return user;
        }

        try
        {
            user = await registerUserAsync(payload);
            logger.LogDebug("[{location}]: Returning new profile for {email}", nameof(AuthService), payload.Email);
            return user;
        }
        catch
        {
            throw;
        }
    }
    public Task<bool> ValidateTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    private async Task<User> registerUserAsync(GoogleJsonWebSignature.Payload payload)
    {
        try
        {
            logger.LogWarning("[{location}]: A user profile for {email} was not found", nameof(AuthService), payload.Email);
            logger.LogDebug("[{location}]: Creating new user profile for {email}", nameof(AuthService), payload.Email);

            var user = await dbService.AddUserAsync(new User(payload.Email, payload.GivenName, payload.FamilyName, payload.Picture));
            logger.LogInformation("[{location}]: Profile successfully created for {email}", nameof(AuthService), payload.Email);
            return user;
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not create user profile for {email}. Ex: {ex}", nameof(AuthService), payload.Email, ex);
            throw;
        }
    }
}