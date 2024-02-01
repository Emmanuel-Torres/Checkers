using checkers_api.Models.GameModels;
using checkers_api.Models.Requests;
using checkers_api.Services.GameManager;
using checkers_api.Services.Matchmaking;
using Microsoft.AspNetCore.SignalR;

namespace checkers_api.Hubs;

public class CheckersHub : Hub<ICheckersHub>
{
    private readonly ILogger<CheckersHub> _logger;
    private readonly IGameManager _gameService;
    private readonly IMatchmakingService _matchmakingService;
    public CheckersHub(ILogger<CheckersHub> logger, IGameManager gameService, IMatchmakingService matchmakingService)
    {
        _logger = logger;
        _gameService = gameService;
        _matchmakingService = matchmakingService;

        _matchmakingService.OnPlayersMatched = StartGameAsync;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogDebug("[{location}]: Player {connectionId} connected to the server", nameof(CheckersHub), Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception is not null)
        {
            _logger.LogError("[{location}]: Player {connectionId} disconnected from the sever due to an exception. Ex: {ex}", nameof(CheckersHub), Context.ConnectionId, exception);
        }
        else
        {
            _logger.LogDebug("[{location}]: Player {connectionId} disconnected from the server", nameof(CheckersHub), Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task FindGameAsync()
    {
        try
        {
            _logger.LogDebug("[{location}]: Player {connectionId} requested matchmaking", nameof(CheckersHub), Context.ConnectionId);

            await _matchmakingService.StartMatchmakingAsync(new Player(Context.ConnectionId, "Guest"));
            await Clients.Client(Context.ConnectionId).SendMessageAsync("server", "You are matchmaking");

            _logger.LogDebug("[{location}]: Player {connectionId} is matchmaking successfully", nameof(CheckersHub), Context.ConnectionId);
        }
        catch (Exception ex)
        {
            _logger.LogError("[{location}]: Matchmaking failed for player {connectionId}. Ex: {ex}", nameof(CheckersHub), Context.ConnectionId, ex);
            await Clients.Client(Context.ConnectionId).SendMessageAsync("server", "Matchmaking failed");
        }
    }

    public async Task MakeMoveAsync(MoveRequest moveRequest)
    {
        try
        {
            _logger.LogDebug("[{location}]: Player {connectionId} made a move request from ({sRow}, {sCol}) to ({dRow}, {dCol})",
                nameof(CheckersHub), Context.ConnectionId, moveRequest.Source.Row, moveRequest.Source.Column, moveRequest.Destination.Row, moveRequest.Destination.Column);

            var res = _gameService.MakeMove(Context.ConnectionId, moveRequest);
            if (res.IsGameOver)
            {
                _logger.LogInformation("[{location}]: Move request from player {connectionId} successfully won the game", nameof(CheckersHub), Context.ConnectionId);
                await EndGameAsync(res.GameId);
                return;
            }
            if (res.WasMoveSuccessful)
            {
                _logger.LogDebug("[{location}]: Move request from player {connectionId} was successful", nameof(CheckersHub), Context.ConnectionId);
                await Clients.Client(Context.ConnectionId).MoveSuccessfulAsync(res.Board);
                return;
            }

            await Clients.Client(Context.ConnectionId).SendMessageAsync("server", "Move was not successful");
        }
        catch (Exception ex)
        {
            _logger.LogError("[{location}]: Could not make move. Ex: {ex}", nameof(CheckersHub), ex);
            await Clients.Client(Context.ConnectionId).SendMessageAsync("server", "Something went wrong when making your move");
        }
    }

    public async Task MoveCompletedAsync()
    {
        try
        {
            var game = _gameService.GetGameByPlayerId(Context.ConnectionId);
            if (game is null)
            {
                throw new Exception("Player is not in a game");
            }
            var currentTurn = game.Players.First(p => p.PlayerId != Context.ConnectionId);

            _logger.LogDebug("[{location}]: Player {p1} is done moving. Passing turn to {p2}", nameof(CheckersHub), Context.ConnectionId, currentTurn.PlayerId);
            await Clients.Client(currentTurn.PlayerId).YourTurnToMoveAsync(game.Board);
        }
        catch (Exception ex)
        {
            _logger.LogError("[{location}]: Could complete move for player {connectionId}. Ex: {ex}", nameof(CheckersHub), Context.ConnectionId, ex);
        }
    }

    public async Task GetValidMovesAsync(Location source)
    {
        try
        {
            _logger.LogDebug("[{location}]: Getting valid locations for ({row}, {column})", nameof(CheckersHub), source.Row, source.Column);
            var res = _gameService.GetValidMoves(Context.ConnectionId, source);
            _logger.LogDebug("[{location}]: Found {count} valid locations for ({row}, {column})", nameof(CheckersHub), res.Count(), source.Row, source.Column);

            await Clients.Client(Context.ConnectionId).SendValidMoveLocationsAsync(res);
        }
        catch (Exception ex)
        {
            _logger.LogError("[{location}]: Could not get valid moves for source ({column}, {row}). Ex: {ex}", nameof(CheckersHub), source.Column, source.Row, ex);
        }
    }

    private async Task StartGameAsync(Player p1, Player p2)
    {
        try
        {
            _logger.LogInformation("[{location}]: Starting game for players {p1} and {p2}", nameof(CheckersHub), p1.PlayerId, p2.PlayerId);

            var gameId = _gameService.StartGame(p1, p2);
            var game = _gameService.GetGameByGameId(gameId);

            foreach (var p in game!.Players)
            {
                await Clients.Client(p.PlayerId).SendMessageAsync("server", "You were successfully matchmade");
                await Clients.Client(p.PlayerId).SendJoinConfirmationAsync(p.Name, game.Board);
            }

            await Clients.Client(p1.PlayerId).YourTurnToMoveAsync(game.Board);
        }
        catch (Exception ex)
        {
            _logger.LogError("[{location}]: Could not start game for players {p1} and {p2}. Ex: {ex}", nameof(CheckersHub), p1.PlayerId, p2.PlayerId, ex);
        }
    }

    private async Task EndGameAsync(string gameId)
    {
        try
        {
            _logger.LogDebug("[{location}]: Ending game {id}", nameof(CheckersHub), gameId);
            // var results = _gameService.TerminateGame(gameId);
            // foreach (var p in results.Players)
            // {
            //     await Clients.Client(p.PlayerId).GameOverAsync(results.Winner.Name, results.Board);
            // }

            // _logger.LogInformation("[{location}]: Game {id} was successfully terminated. Winner {connectionId}", nameof(CheckersHub), gameId);
        }
        catch (Exception ex)
        {
            _logger.LogError("[{location}]: Could not end game properly. Ex: {ex}", nameof(CheckersHub), ex);
        }
    }
}