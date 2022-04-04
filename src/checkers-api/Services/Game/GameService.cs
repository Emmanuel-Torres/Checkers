using System.Collections.Concurrent;
using checkers_api.GameLogic;
using checkers_api.Models.GameModels;


namespace checkers_api.Services;

public class GameService : IGameService
{
    private readonly ConcurrentDictionary<string, Player> activePlayers;
    private readonly ConcurrentDictionary<string, string> playerGame;
    private readonly ConcurrentDictionary<string, IGame> activeGames;
    private readonly ILogger<GameService> logger;
    private readonly ILoggerFactory loggerFactory;

    public GameService(ILogger<GameService> logger, ILoggerFactory loggerFactory)
    {
        activePlayers = new ConcurrentDictionary<string, Player>();
        playerGame = new ConcurrentDictionary<string, string>();
        activeGames = new ConcurrentDictionary<string, IGame>();
        this.logger = logger;
        this.loggerFactory = loggerFactory;
    }

    public string StartGame(Player player1, Player player2)
    {
        ArgumentNullException.ThrowIfNull(player1);
        ArgumentNullException.ThrowIfNull(player2);

        if (!activePlayers.TryAdd(player1.PlayerId, player1))
        {
            throw new Exception($"Player {player1.PlayerId} is already active in another game");
        }
        if (!activePlayers.TryAdd(player2.PlayerId, player2))
        {
            throw new Exception($"Player {player2.PlayerId} is already active in another game");
        }

        var game = new Game(player1, player2, loggerFactory.CreateLogger<Game>());
        if (!activeGames.TryAdd(game.Id, game))
        {
            throw new Exception($"Game {game.Id} already exists");
        }

        while (!playerGame.TryAdd(player1.PlayerId, game.Id))
        {
            if (!playerGame.TryRemove(player1.PlayerId, out var _))
            {
                throw new Exception($"Player {player1.PlayerId} could not be removed from player-game dictionary");
            }
        }
        while (!playerGame.TryAdd(player2.PlayerId, game.Id))
        {
            if (!playerGame.TryRemove(player2.PlayerId, out var _))
            {
                throw new Exception($"Player {player2.PlayerId} could not be removed from player-game dictionary");
            }
        }

        return game.Id;
    }

    public IGame? GetGameByGameId(string gameId)
    {
        ArgumentNullException.ThrowIfNull(gameId);

        activeGames.TryGetValue(gameId, out var game);
        return game;
    }

    public IGame? GetGameByPlayerId(string playerId)
    {
        ArgumentNullException.ThrowIfNull(playerId);

        if (playerGame.TryGetValue(playerId, out var gameId))
        {
            activeGames.TryGetValue(gameId, out var game);
            return game;
        }
        else
        {
            return null;
        }
    }

    public MoveResult MakeMove(string playerId, MoveRequest moveRequest)
    {
        ArgumentNullException.ThrowIfNull(playerId);
        ArgumentNullException.ThrowIfNull(moveRequest);

        var game = GetGameByPlayerId(playerId);
        var moveSuccessful = false;

        if (game is null)
        {
            throw new Exception($"Player {playerId} is not associated with a game");
        }
        if (game.State == GameState.GameOver)
        {
            throw new Exception("Game is already over");
        }

        try
        {
            game.MakeMove(playerId, moveRequest);
            moveSuccessful = true;
        }
        catch (Exception ex)
        {
            logger.LogWarning("[{location}]: Could not complete move request from player {playerId}. Ex: {ex}", nameof(GameService), playerId, ex);
            moveSuccessful = false;
        }

        return new MoveResult(game.Id, moveSuccessful, game.IsGameOver(), game.Board);
    }

    public IEnumerable<Location> GetValidMoves(string playerId, Location location)
    {
        try
        {
            var game = GetGameByPlayerId(playerId);
            if (game is null)
            {
                throw new Exception("Game does not exist");
            }

            return game.GetValidMoves(playerId, location);
        }
        catch (Exception ex)
        {
            logger.LogWarning("[{location}]: Could not get valid moves for location ({row}. {column}). Ex: {ex}", nameof(GameService), location.Row, location.Column, ex);
            return new List<Location>();
        }
    }
    public GameResults QuitGame(string playerId)
    {
        // ArgumentNullException.ThrowIfNull(playerId);

        // var game = GetGameByPlayerId(playerId);

        // if (game is null)
        // {
        //     throw new Exception($"Player {playerId} is not associated with a game");
        // }

        // //TODO: Add logic to signal game that a player quited.

        // TerminateGame(game.Id);

        // return GenerateGameResults(game.Id);
        throw new NotImplementedException();
    }

    public GameResults TerminateGame(string gameId)
    {
        ArgumentNullException.ThrowIfNull(gameId);

        var game = GetGameByGameId(gameId);
        if (game is null)
        {
            throw new Exception($"Game {gameId} does not exists");
        }
        if (game.State == GameState.Ongoing)
        {
            throw new Exception($"Game {gameId} is still ongoing");
        }

        var players = game.Players;
        foreach (var p in players)
        {
            try
            {
                RemovePlayer(p.PlayerId);
            }
            catch (Exception ex)
            {
                logger.LogError("[{location}]: Could not terminate game {gameId}. Ex: {ex}", nameof(GameService), gameId, ex);
                throw;
            }
        }

        var results = game.GetGameResults();
        if (results is null)
        {
            throw new Exception("Results were not generated");
        }

        if (!activeGames.TryRemove(gameId, out var _))
        {
            throw new Exception("Could not remove game from active games");
        }

        return results;
    }

    private void RemovePlayer(string playerId)
    {
        ArgumentNullException.ThrowIfNull(playerId);

        if (!activePlayers.TryRemove(playerId, out var _))
        {
            throw new Exception($"Player {playerId} was not active");
        }

        if (!playerGame.TryRemove(playerId, out var _))
        {
            throw new Exception($"Player {playerId} was not associated with a game");
        }
    }
}
