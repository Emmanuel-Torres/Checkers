using Microsoft.AspNetCore.Mvc;

namespace checkers_api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class GameController : ControllerBase
{
    private readonly ILogger<GameController> logger;

    public GameController(ILogger<GameController> logger)
    {
        this.logger = logger;
    }

    [HttpPost]
    public Task<ActionResult> MatchMake([FromHeader] string authorization)
    {
        if (!string.IsNullOrEmpty(authorization))
        {
            authorization = authorization.Remove(0, 7);
        }
        throw new NotImplementedException();
    }
}