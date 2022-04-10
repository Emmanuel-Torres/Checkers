using System.Net.Http.Headers;
using checkers_api.Models.ExternalModels;
using checkers_api.Models.PersistentModels;
using checkers_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace checkers_api.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> logger;
    private readonly IAuthService authService;

    public AuthController(ILogger<AuthController> logger, IAuthService authService)
    {
        this.logger = logger;
        this.authService = authService;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<UserProfile?>> GetProfile([FromHeader] string authorization)
    {
        try
        {
            authorization = authorization.Remove(0, 7);
            logger.LogInformation("[{location}]: Received request to get profile", nameof(AuthController));
            return await authService.GetUserAsync(authorization);
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not get user profile: {ex}", nameof(AuthController), ex);
            return StatusCode(500);
        }
    }

    [HttpPut("profile")]
    public async Task<ActionResult> UpdateProfile([FromForm] ProfileUpdateRequest request)
    {
        try
        {
            logger.LogDebug("[{location}]: Received a request to update a profile", nameof(AuthController));
            var authorization = HttpContext.Request.Headers.Authorization;
            if (!AuthenticationHeaderValue.TryParse(authorization, out var token))
            {
                throw new Exception("Not a valid token");
            }

            await authService.UpdateProfileAsync(token.Parameter!, request);
            logger.LogInformation("[{location}]: Successfully updated profile", nameof(AuthController));
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not update profile. Ex: {ex}", nameof(AuthController), ex);
            return StatusCode(500);
        }
    }

    // [HttpPost("logout")]
    // public async Task<ActionResult> Logout([FromHeader] string authorization)
    // {
    //     try
    //     {
    //         authorization = authorization.Remove(0, 7);
    //         await authService.Logout(authorization);
    //         return Ok();
    //     }
    //     catch (Exception ex)
    //     {
    //         logger.LogError("[{location}]: Could not logout user. Ex: {ex}", nameof(AuthController), ex);
    //         return StatusCode(500);
    //     }
    // }
}