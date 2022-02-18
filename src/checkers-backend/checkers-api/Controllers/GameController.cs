using Microsoft.AspNetCore.Mvc;

namespace checkers_api.Controllers;

[ApiController]
[Route("/api/[controller")]
public class GameController : ControllerBase
{
    private readonly ILogger<GameController> logger;

    public GameController(ILogger<GameController> logger)
    {
        this.logger = logger;
    }

    [HttpPost]
    public Task<ActionResult> MatchMake()
    {
        throw new NotImplementedException();
        // try
        // {

        // }
        // catch (Exception ex)
        // {
        //     logger.LogError("Could not matchmake player. Ex: {ex}", ex);
        // }
    }
}