using System.Collections.Concurrent;
using checkers_api.Models.GameLogic;
using checkers_api.Models.GameModels;
using checkers_api.Models.Requests;
using checkers_api.Models.Responses;

namespace checkers_api.Services.GameManager;

public class GameManager : IGameManager
{
    private readonly ILogger<GameManager> _logger;
    private readonly ConcurrentDictionary<string, Game> _games;

    public GameManager(ILogger<GameManager> logger)
    {
        _logger = logger;
        _games = new();
    }

    public GameInfo CreateGame(Player player1, Player player2)
    {
        var gameId = Guid.NewGuid().ToString();
        var game = new Game(gameId, player1, player2);
        var board = game.Board;
        var currentTurnId = game.CurrentTurn.PlayerId;
        _games.TryAdd(gameId, game);

        return new GameInfo(gameId, board, currentTurnId);
    }

    public GameInfo MakeMove(string playerId, MoveRequest moveRequest)
    {
        throw new NotImplementedException();
    }
}
