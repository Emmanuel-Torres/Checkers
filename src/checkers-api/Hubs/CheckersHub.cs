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
        this.matchmakingService.ConfigureQueue(StartGameAsync);

        this.logger.LogInformation("[{location}]: Successfully created hub", nameof(CheckersHub));
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
            logger.LogDebug("[{location}]: Player {token} requested matchmaking", nameof(CheckersHub), Context.ConnectionId);
            var profile = await authService.GetUserAsync(token ?? "");
            if (profile is not null)
            {
                logger.LogDebug("[{location}]: User profile was found for player {token}. Matchmaking as {name}", nameof(CheckersHub), Context.ConnectionId, profile.GivenName);
                await matchmakingService.MatchMakeAsync(new Player(Context.ConnectionId, profile.GivenName));
            }
            else
            {
                logger.LogDebug("[{location}]: User profile was not found for player {token}. Matchmaking as guest.", nameof(CheckersHub), Context.ConnectionId);
                await matchmakingService.MatchMakeAsync(new Player(Context.ConnectionId, "Guest"));
            }

            await Clients.Client(Context.ConnectionId).SendMessageAsync("server", "You are matchmaking");
            logger.LogDebug("[{location}]: Player {token} is matchmaking successfully", nameof(CheckersHub), Context.ConnectionId);
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not matchmake player {token}. Ex: {ex}", nameof(CheckersHub), Context.ConnectionId, ex);
            await Clients.Client(Context.ConnectionId).SendMessageAsync("server", "Matchmaking failed");
        }
    }

    public async Task MakeMoveAsync(MoveRequest moveRequest)
    {
        try
        {
            var res = gameService.MakeMove(Context.ConnectionId, moveRequest);

            if (res.IsGameOver)
            {
                await EndGameAsync(res.GameId);
                return;
            }
            if (res.WasMoveSuccessful)
            {
                await Clients.Client(Context.ConnectionId).MoveSuccessfulAsync(res.Board);
                return;
            }

            await Clients.Client(Context.ConnectionId).SendMessageAsync("server", "Move was not successful");
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not make move. Ex: {ex}", nameof(CheckersHub), ex);
        }
    }

    public async Task GetValidMovesAsync(Location source)
    {
        try
        {
            var res = gameService.GetValidMoves(Context.ConnectionId, source);
            await Clients.Client(Context.ConnectionId).SendValidMoveLocationsAsync(res);
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not get valid moves for source ({column}, {row}). Ex: {ex}", nameof(CheckersHub), source.Column, source.Row, ex);
        }
    }

    private async Task StartGameAsync(Player p1, Player p2)
    {
        try
        {
            var gameId = gameService.StartGame(p1, p2);
            var game = gameService.GetGameByGameId(gameId);

            logger.LogInformation("[{location}]: Starting game {gameId} with players {p1} and {p2}", nameof(CheckersHub), gameId, p1.PlayerId, p2.PlayerId);

            foreach (var p in game!.Players)
            {
                await Clients.Client(p.PlayerId).SendMessageAsync("server", "You were successfully matchmade");
                await Clients.Client(p.PlayerId).SendJoinConfirmationAsync(p.Name + p.PlayerId, game.Board.Squares);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not start game for players {p1} and {p2}. Ex: {ex}", nameof(CheckersHub), p1.PlayerId, p2.PlayerId, ex);
        }
    }

    private async Task EndGameAsync(string gameId)
    {
        try
        {
            var results = gameService.TerminateGame(gameId);
            foreach (var p in results.Players)
            {
                await Clients.Client(p.PlayerId).GameOverAsync(results);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not end game properly. Ex: {ex}", nameof(CheckersHub), ex);
        }
    }
}