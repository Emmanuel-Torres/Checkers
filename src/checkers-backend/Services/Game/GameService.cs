using System.Collections.Concurrent;
using checkers_game;

namespace checkers_api.Services;

public class GameService : IGameService
{
    private readonly ConcurrentQueue<Player> matchMakingQueue;
    private readonly ILogger<GameService> logger;
    private readonly IAuthService authService;

    public GameService(ILogger<GameService> logger, IAuthService authService)
    {
        matchMakingQueue = new ConcurrentQueue<Player>();
        this.logger = logger;
        this.authService = authService;
    }
    public async Task<Player> MatchMakeAsync(string? token)
    {
        logger.LogInformation("[{location}]: Trying to matchmake new player", nameof(GameService));
        var player = new Player(Guid.NewGuid().ToString(), "Anonymous");

        if (token is not null)
        {
            try
            {
                logger.LogDebug("[{location}]: Retrieving user profile for matchmaking.", nameof(GameService));
                var profile = await authService.GetUserAsync(token);
                player.Name = profile.GivenName;
                logger.LogDebug("[{location}]: User profile successfully retrieved.", nameof(GameService));
            }
            catch (Exception ex)
            {
                logger.LogError("[{location}]: Could not retreive user profile for matchmaking. Ex: {ex}", nameof(GameService), ex);
                throw;
            }
        }

        matchMakingQueue.Enqueue(player);
        return player;
    }
}
