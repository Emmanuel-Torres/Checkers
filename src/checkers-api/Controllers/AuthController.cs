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
    public async Task<ActionResult<UserProfile>> GetProfile([FromHeader] string authorization)
    {
        authorization = authorization.Remove(0, 7);
        try
        {
            logger.LogInformation("[{location}]: Received request to get profile", nameof(AuthController));
            return await authService.GetUserAsync(authorization);
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not get user profile: {ex}", nameof(AuthController), ex);
            return StatusCode(500);
        }
    }
}