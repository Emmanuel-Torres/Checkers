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

        var gameId = tryStartGame();
        return gameId;
    }

    public IGame GetGameByGameId(string gameId)
    {
        throw new NotImplementedException();
    }

    public IGame GetGameByPlayerId(string playerId)
    {
        throw new NotImplementedException();
    }

    public int MakeMove(string playerId, MoveRequest moveRequest)
    {
        throw new NotImplementedException();
    }

    private string? tryStartGame()
    {
        // try
        // {
        //     if (matchMakingQueue.Count > 1 && matchMakingQueue.TryDequeue(out var p1) && matchMakingQueue.TryDequeue(out var p2))
        //     {
        //         IGame game = new Game(p1, p2);
        //         activeGames.TryAdd(game.GameId, game);
        //         playerGame.TryAdd(p1.PlayerId, game.GameId);
        //         playerGame.TryAdd(p2.PlayerId, game.GameId);
        //         activePlayers.TryUpdate(p1.PlayerId, updateValueFac);

        //         return game.GameId;
        //     }
        //     return null;
        // }
        // catch (Exception ex)
        // {
        //     throw;
        // }

        throw new NotImplementedException();
    }
}
