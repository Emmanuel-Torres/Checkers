using Microsoft.AspNetCore.Mvc;

namespace checkers_api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> logger;

    public UserController(ILogger<UserController> logger)
    {
        this.logger = logger;
    }

    
}