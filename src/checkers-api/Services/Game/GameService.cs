using System.Collections.Concurrent;
using checkers_api.Models.DomainModels;
using checkers_api.Models.GameModels;

namespace checkers_api.Services;

public class GameService : IGameService
{
    private readonly ConcurrentDictionary<Id, Player> activePlayers;
    private readonly ConcurrentDictionary<Id, Id> playerGame;
    private readonly ConcurrentDictionary<Id, IGame> activeGames;
    private readonly ILogger<GameService> logger;

    public GameService(ILogger<GameService> logger)
    {
        activePlayers = new ConcurrentDictionary<Id, Player>();
        playerGame = new ConcurrentDictionary<Id, Id>();
        activeGames = new ConcurrentDictionary<Id, IGame>();
        this.logger = logger;
    }

    public Id StartGame(Player player1, Player player2)
    {
        ArgumentNullException.ThrowIfNull(player1);
        ArgumentNullException.ThrowIfNull(player2);

        if (!activePlayers.TryAdd(player1.PlayerId, player1))
        {
            throw new Exception($"Player {player1.PlayerId.Value} is already active in another game");
        }
        if (!activePlayers.TryAdd(player2.PlayerId, player2))
        {
            throw new Exception($"Player {player2.PlayerId.Value} is already active in another game");
        }

        var game = new Game(player1, player2);
        if (!activeGames.TryAdd(game.GameId, game))
        {
            throw new Exception($"Game {game.GameId.Value} already exists");
        }

        while (!playerGame.TryAdd(player1.PlayerId, game.GameId))
        {
            if (!playerGame.TryRemove(player1.PlayerId, out var _))
            {
                throw new Exception($"Player {player1.PlayerId} could not be removed from player-game dictionary");
            }
        }
        while (!playerGame.TryAdd(player2.PlayerId, game.GameId))
        {
            if (!playerGame.TryRemove(player2.PlayerId, out var _))
            {
                throw new Exception($"Player {player2.PlayerId} could not be removed from player-game dictionary");
            }
        }

        return game.GameId;
    }

    public IGame? GetGameByGameId(Id gameId)
    {
        ArgumentNullException.ThrowIfNull(gameId);

        activeGames.TryGetValue(gameId, out var game);
        return game;
    }

    public IGame? GetGameByPlayerId(Id playerId)
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

    //TODO: Still need to modify how make move returns true of false from move
    public GameState TryMakeMove(Id playerId, MoveRequest moveRequest)
    {
        ArgumentNullException.ThrowIfNull(playerId);
        ArgumentNullException.ThrowIfNull(moveRequest);

        if (!playerGame.TryGetValue(playerId, out var gameId))
        {
            throw new Exception("Player was not in a game");
        }
        if (!activeGames.TryGetValue(gameId, out var game))
        {
            throw new Exception("Game does not exist");
        }
        if (game.State == GameState.GameOver)
        {
            throw new Exception("Game is already over");
        }

        try
        {
            game.MakeMove(playerId, moveRequest);
            return game.State;
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not complete move request from player {playerId}. Ex: {ex}", nameof(GameService), playerId, ex);
            throw;
        }
    }

    public void RemovePlayerFromGame(Id playerId)
    {
        ArgumentNullException.ThrowIfNull(playerId);

        if (!activePlayers.TryRemove(playerId, out var _))
        {
            throw new Exception($"Player {playerId.Value} was not active");
        }

        if (!playerGame.TryRemove(playerId, out var gameId))
        {
            throw new Exception($"Player {playerId.Value} was not associated with any games");
        }

        if (!activeGames.TryGetValue(gameId, out var game))
        {
            throw new Exception($"Game {gameId} did not exist");
        }

        if (game.State == GameState.Ongoing)
        {
            // DO GAME OVER LOGIC
        }
        else
        {
            throw new Exception($"Game {gameId.Value} is already over");
        }
    }
}
