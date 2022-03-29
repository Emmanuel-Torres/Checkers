using checkers_api.Models.GameModels;
using checkers_api.Models.PersistentModels;
using checkers_api.Services;
using Microsoft.AspNetCore.SignalR;

namespace checkers_api.Hubs;

public class CheckersHub : Hub<ICheckersHub>
{
    private readonly ILogger<CheckersHub> logger;
    private readonly IGameService gameService;
    private readonly IMatchmakingService matchmakingService;
    private readonly IAuthService authService;

    public CheckersHub(ILogger<CheckersHub> logger, IGameService gameService, IMatchmakingService matchmakingService, IAuthService authService)
    {
        this.logger = logger;
        this.gameService = gameService;
        this.matchmakingService = matchmakingService;
        this.authService = authService;
        this.matchmakingService.ConfigureQueue(StartGame);
    }

    public override async Task OnConnectedAsync()
    {
        logger.LogDebug("[{location}]: Player {token} connected to the server", nameof(CheckersHub), Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception is not null)
        {
            logger.LogError("[{location}]: Player {token} disconnected from the sever due to an exception. Ex: {ex}", nameof(CheckersHub), Context.ConnectionId, exception);
        }
        else
        {
            logger.LogDebug("[{location}]: Player {token} disconnected from the server", nameof(CheckersHub), Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task MatchMakeAsync(string? token)
    {
        try
        {
            var profile = await authService.GetUserAsync(token ?? "");
            if (profile is not null)
            {
                await matchmakingService.MatchMakeAsync(new Player(Context.ConnectionId, profile.GivenName));
            }
            else
            {
                await matchmakingService.MatchMakeAsync(new Player(Context.ConnectionId, "Guest"));
            }

            // Tell client that matchmaking succeeded.
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not matchmake player {token}. Ex: {ex}", nameof(CheckersHub), Context.ConnectionId, ex);
            //Tell the client that something went wrong.
        }
    }

    public Task MakeMoveAsync(MoveRequest moveRequest)
    {
        throw new NotImplementedException();
    }

    private void StartGame(Player p1, Player p2)
    {
        try
        {
            var gameId = gameService.StartGame(p1, p2);
            var game = gameService.GetGameByGameId(gameId);

            foreach (var p in game!.Players)
            {
                Clients.Client(p.PlayerId.Value).SendMessage("You were successfully matchmade");
            }
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not start game for players {p1} and {p2}. Ex: {ex}", nameof(CheckersHub), p1.PlayerId.Value, p2.PlayerId.Value, ex);
        }
    }
}