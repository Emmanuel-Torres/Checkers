using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace checkers_api.Controllers;

[ApiController]
[Route("/api/[contoller]")]
[Authorize]
public class RegistrationController : ControllerBase
{
    private readonly ILogger<RegistrationController> _logger;

    public RegistrationController(ILogger<RegistrationController> logger)
    {
        this._logger = logger;
    }

    [HttpGet("register")]
    public Task<IActionResult> Register(string token)
    {
        throw new NotImplementedException();
    }
}