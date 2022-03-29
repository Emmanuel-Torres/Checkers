using checkers_api.Models.GameModels;
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

        this.logger.LogInformation("[{location}]: Successfully created hub", nameof(CheckersHub));
    }

    public override async Task OnConnectedAsync()
    {
        logger.LogInformation("[{location}]: Player {token} connected to the server", nameof(CheckersHub), Context.ConnectionId);
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

            await Clients.Client(Context.ConnectionId).SendMessage("You are matchmaking");
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not matchmake player {token}. Ex: {ex}", nameof(CheckersHub), Context.ConnectionId, ex);
            await Clients.Client(Context.ConnectionId).SendMessage("Matchmaking failed");
        }
    }

    public async Task MakeMoveAsync(MoveRequest moveRequest)
    {
        try
        {
            var res = gameService.MakeMove(Context.ConnectionId, moveRequest);

            if (res.IsGameOver)
            {
                await EndGame(res.GameId);
                return;
            }
            if (res.WasMoveSuccessful)
            {
                await Clients.Client(Context.ConnectionId).MoveSuccessful(res.Board);
                return;
            }

            await Clients.Client(Context.ConnectionId).SendMessage("Move was not successful");
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not make move. Ex: {ex}", nameof(CheckersHub), ex);
        }
    }

    private async Task StartGame(Player p1, Player p2)
    {
        try
        {
            var gameId = gameService.StartGame(p1, p2);
            var game = gameService.GetGameByGameId(gameId);

            foreach (var p in game!.Players)
            {
                await Clients.Client(p.PlayerId).SendMessage("You were successfully matchmade");
                await Clients.Client(p.PlayerId).JoinConfirmation(p.Name, game.Board.Squares);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not start game for players {p1} and {p2}. Ex: {ex}", nameof(CheckersHub), p1.PlayerId, p2.PlayerId, ex);
        }
    }

    private async Task EndGame(string gameId)
    {
        try
        {
            var results = gameService.TerminateGame(gameId);
            foreach (var p in results.Players)
            {
                await Clients.Client(p.PlayerId).GameOver(results);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not end game properly. Ex: {ex}", nameof(CheckersHub), ex);
        }
    }
}