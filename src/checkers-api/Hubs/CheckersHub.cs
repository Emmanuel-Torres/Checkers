using System.Collections.Concurrent;
using System.Text;
using Azure.Messaging.ServiceBus;
using checkers_api.Models.GameModels;
using checkers_api.Services;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace checkers_api.Hubs;

public class CheckersHub : Hub<ICheckersHub>
{
    private readonly ILogger<CheckersHub> logger;
    private readonly IGameService gameService;
    private readonly IAuthService authService;
    private readonly IMatchmakingService matchmakingService;
    public CheckersHub(ILogger<CheckersHub> logger, IGameService gameService, IAuthService authService, IMatchmakingService matchmakingService)
    {
        this.logger = logger;
        this.gameService = gameService;
        this.authService = authService;
        this.matchmakingService = matchmakingService;
        this.matchmakingService.ConfigureQueue(StartGameAsync);
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
            logger.LogDebug("[{location}]: Player {token} made a move request from ({sRow}, {sCol}) to ({dRow}, {dCol})",
                nameof(CheckersHub), Context.ConnectionId, moveRequest.Source.Row, moveRequest.Source.Column, moveRequest.Destination.Row, moveRequest.Destination.Column);

            var res = gameService.MakeMove(Context.ConnectionId, moveRequest);
            if (res.IsGameOver)
            {
                logger.LogInformation("[{location}]: Move request from player {token} successfully won the game", nameof(CheckersHub), Context.ConnectionId);
                await EndGameAsync(res.GameId);
                return;
            }
            if (res.WasMoveSuccessful)
            {
                logger.LogDebug("[{location}]: Move request from player {token} was successful", nameof(CheckersHub), Context.ConnectionId);
                await Clients.Client(Context.ConnectionId).MoveSuccessfulAsync(res.Board);
                return;
            }

            await Clients.Client(Context.ConnectionId).SendMessageAsync("server", "Move was not successful");
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not make move. Ex: {ex}", nameof(CheckersHub), ex);
            await Clients.Client(Context.ConnectionId).SendMessageAsync("server", "Something went wrong when making your move");
        }
    }

    public async Task MoveCompletedAsync()
    {
        try
        {
            var game = gameService.GetGameByPlayerId(Context.ConnectionId);
            if (game is null)
            {
                throw new Exception("Player is not in a game");
            }
            var currentTurn = game.Players.First(p => p.PlayerId != Context.ConnectionId);

            logger.LogDebug("[{location}]: Player {p1} is done moving. Passing turn to {p2}", nameof(CheckersHub), Context.ConnectionId, currentTurn.PlayerId);
            await Clients.Client(currentTurn.PlayerId).YourTurnToMoveAsync(game.Board);
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could complete move for player {token}. Ex: {ex}", nameof(CheckersHub), Context.ConnectionId, ex);
        }
    }

    public async Task GetValidMovesAsync(Location source)
    {
        try
        {
            logger.LogDebug("[{location}]: Getting valid locations for ({row}, {column})", nameof(CheckersHub), source.Row, source.Column);
            var res = gameService.GetValidMoves(Context.ConnectionId, source);
            logger.LogDebug("[{location}]: Found {count} valid locations for ({row}, {column})", nameof(CheckersHub), res.Count(), source.Row, source.Column);

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
            logger.LogInformation("[{location}]: Starting game for players {p1} and {p2}", nameof(CheckersHub), p1.PlayerId, p2.PlayerId);

            var gameId = gameService.StartGame(p1, p2);
            var game = gameService.GetGameByGameId(gameId);

            foreach (var p in game!.Players)
            {
                var color = p.PlayerId == p1.PlayerId ? Color.Black : Color.White;
                await Clients.Client(p.PlayerId).SendMessageAsync("server", "You were successfully matchmade");
                await Clients.Client(p.PlayerId).SendJoinConfirmationAsync(p.Name, color, game.Board);
            }

            await Clients.Client(p1.PlayerId).YourTurnToMoveAsync(game.Board);
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
            logger.LogDebug("[{location}]: Ending game {id}", nameof(CheckersHub), gameId);
            var results = gameService.TerminateGame(gameId);
            foreach (var p in results.Players)
            {
                await Clients.Client(p.PlayerId).GameOverAsync(results.Winner.Name, results.Board);
            }

            logger.LogInformation("[{location}]: Game {id} was successfully terminated. Winner {token}", nameof(CheckersHub), gameId, results.Winner.PlayerId);
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not end game properly. Ex: {ex}", nameof(CheckersHub), ex);
        }
    }
}