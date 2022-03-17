using System.Collections.Concurrent;
using checkers_api.GameModels;
namespace checkers_api.Services;

public class GameService : IGameService
{
    private readonly ConcurrentDictionary<string, PlayerStatus> activePlayers;
    private readonly ConcurrentDictionary<string, string> playerGame;
    private readonly ConcurrentDictionary<string, IGame> activeGames;
    private readonly ConcurrentQueue<Player> matchMakingQueue;
    private readonly ILogger<GameService> logger;

    public GameService(ILogger<GameService> logger)
    {
        activePlayers = new ConcurrentDictionary<string, PlayerStatus>();
        playerGame = new ConcurrentDictionary<string, string>();
        activeGames = new ConcurrentDictionary<string, IGame>();
        matchMakingQueue = new ConcurrentQueue<Player>();
        this.logger = logger;
    }

    public string? MatchMakeAsync(Player player)
    {
        if (player is null)
        {
            throw new ArgumentException("Player object was null");
        }

        logger.LogDebug("[{location}]: Trying to matchmake player {playerId}", nameof(GameService), player.PlayerId);

        if (activePlayers.ContainsKey(player.PlayerId))
        {
            activePlayers.TryGetValue(player.PlayerId, out var status);
            throw new Exception($"Player is already active with status {status}");
        }
        matchMakingQueue.Enqueue(player);
        logger.LogDebug("[{location}]: Player {playerId} was added to the queue", nameof(GameService), player.PlayerId);

        string? gameId = tryStartGame();
        return gameId;
    }

    public IGame? GetGameByGameId(string gameId)
    {
        try
        {
            activeGames.TryGetValue(gameId, out var game);
            return game;
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not get game with id {gameId}. Ex: {ex}", nameof(GameService), gameId, ex);
            throw;
        }
    }

    public IGame? GetGameByPlayerId(string playerId)
    {
        try
        {
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
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not get game for player {playerId}. Ex: {ex}", nameof(GameService), playerId, ex);
            throw;
        }

    }

    public int MakeMove(string playerId, MoveRequest moveRequest)
    {
        throw new NotImplementedException();
    }

    private string? tryStartGame()
    {
        try
        {
            Player? p1 = null;
            Player? p2 = null;
            if (matchMakingQueue.Count > 1 && matchMakingQueue.TryDequeue(out p1) && matchMakingQueue.TryDequeue(out p2))
            {
                IGame game = new Game(p1, p2);

                activeGames.TryAdd(game.GameId, game);
                playerGame.TryAdd(p1.PlayerId, game.GameId);
                playerGame.TryAdd(p2.PlayerId, game.GameId);

                activePlayers.Remove(p1.PlayerId, out var _);
                activePlayers.Remove(p2.PlayerId, out var _);

                activePlayers.TryAdd(p1.PlayerId, PlayerStatus.MatchMade);
                activePlayers.TryAdd(p2.PlayerId, PlayerStatus.MatchMade);

                return game.GameId;
            }
            else
            {
                if (p1 is not null)
                {
                    matchMakingQueue.Enqueue(p1);
                }
                if (p2 is not null)
                {
                    matchMakingQueue.Enqueue(p2);
                }
                return null;
            }
        }
        catch (Exception ex)
        {
            logger.LogError("[{location}]: Could not start game. Ex: {ex}", nameof(GameService), ex);
            throw;
        }
    }
}
