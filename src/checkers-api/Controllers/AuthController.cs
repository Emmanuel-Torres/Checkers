using checkers_api.Models;
using checkers_api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace checkers_api.Controllers;

[ApiController]
[Route("/api/[contoller]")]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService authService;

    public AuthController(ILogger<AuthController> logger, IAuthService authService)
    {
        this._logger = logger;
        this.authService = authService;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<User>> GetProfile([FromHeader] string authorization)
    {
        try
        {
            return await authService.GetUserAsync(authorization);
        }
        catch (Exception ex)
        {
            _logger.LogError("Could not get user profile: {ex}", ex);
            return StatusCode(500);
        }
    }
}