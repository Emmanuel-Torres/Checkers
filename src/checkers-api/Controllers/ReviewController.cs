using System.Net.Http.Headers;
using checkers_api.Models.PrimitiveModels;
using checkers_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace checkers_api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly ILogger<ReviewController> logger;
    private readonly IDbService dbService;
    private readonly IAuthService authService;

    public ReviewController(ILogger<ReviewController> logger, IDbService dbService, IAuthService authService)
    {
        this.logger = logger;
        this.dbService = dbService;
        this.authService = authService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
    {
        try
        {
            return Ok(await dbService.GetReviewsAsync());
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not get reviews. Ex {ex}", nameof(ReviewController), ex);
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddReview([FromBody] string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return BadRequest();
        }
        if (content.Trim().Length >= 1000)
        {
            return BadRequest();
        }
        try
        {
            logger.LogDebug("[{location}]: Adding new review", nameof(ReviewController));
            var authorization = HttpContext.Request.Headers.Authorization;
            string? playerId = null;
            if (AuthenticationHeaderValue.TryParse(authorization, out var token))
            {
                playerId = (await authService.GetUserAsync(token.Parameter!))?.Id;
                logger.LogDebug("[{location}]: User profile was found. Id {id}", nameof(ReviewController), playerId);
            }

            await dbService.AddReviewAsync(content, playerId);
            return Ok();
        }
        catch(Exception ex)
        {
            logger.LogError("[{location}]: Could not add review. Ex: {ex}", nameof(ReviewController), ex);
            return StatusCode(500);
        }
    }
}