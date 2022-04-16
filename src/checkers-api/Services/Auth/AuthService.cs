using checkers_api.Models.ExternalModels;
using checkers_api.Models.PersistentModels;
using Google.Apis.Auth;

namespace checkers_api.Services;

public class AuthService : IAuthService
{
    private readonly IUserDbService dbService;
    private readonly ILogger<AuthService> logger;
    private readonly IImageService imageService;
    private readonly string clientId = "203576300472-qleefq8rh358lkekh6c1vhq3222jp8nh.apps.googleusercontent.com";

    public AuthService(IUserDbService dbService, ILogger<AuthService> logger, IImageService imageService)
    {
        this.dbService = dbService;
        this.logger = logger;
        this.imageService = imageService;
    }

    public async Task<DbProfile?> GetUserAsync(string token)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(token);

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

            user = await RegisterUserAsync(payload);
            logger.LogDebug("[{location}]: Returning new profile for {email}", nameof(AuthService), payload.Email);
            return user;
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not get user. Ex: {ex}", nameof(AuthService), ex);
            return null;
        }
    }

    public Task Logout(string token)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateProfileAsync(string token, ProfileUpdateRequest request)
    {
        throw new NotImplementedException();
        // try
        // {
        //     var user = await GetUserAsync(token);

        //     if (user is null)
        //     {
        //         throw new Exception("Could not find user profile to update");
        //     }

        //     var imageUrl = user.Picture;
        //     if (request.Picture is not null)
        //     {
        //         imageUrl = await imageService.SaveImageAsync(request.Picture);
        //     }

        //     logger.LogDebug("[{location}]: User profile for user {email} was found", nameof(AuthService), user.Email);
        //     var updated = new UserProfile(user.Email, user.GivenName, user.FamilyName, imageUrl, request.BestJoke, request.IceCreamFlavor, request.Pizza, request.Age, user.Id);
        //     await dbService.UpdateUserAsync(updated);
        // }
        // catch
        // {
        //     throw;
        // }
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    private async Task<DbProfile> RegisterUserAsync(GoogleJsonWebSignature.Payload payload)
    {
        try
        {
            logger.LogWarning("[{location}]: A user profile for {email} was not found", nameof(AuthService), payload.Email);
            logger.LogDebug("[{location}]: Creating new user profile for {email}", nameof(AuthService), payload.Email);

            var id = IdGenerator.GetId();
            await dbService.AddUserAsync(new DbProfile(id, payload.Email, payload.GivenName, payload.FamilyName, payload.Picture));
            var profile = await dbService.GetUserByIdAsync(id);
            logger.LogInformation("[{location}]: Profile successfully created for {email}", nameof(AuthService), payload.Email);
            return profile!;
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not create user profile for {email}. Ex: {ex}", nameof(AuthService), payload.Email, ex);
            throw;
        }
    }
}