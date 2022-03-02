using System.Collections.Concurrent;
using checkers_api.GameModels;
namespace checkers_api.Services;

public class GameService : IGameService
{
    private readonly ConcurrentDictionary<string, PlayerStatus> activePlayers;
    private readonly ConcurrentQueue<Player> matchMakingQueue;
    private readonly ILogger<GameService> logger;

    public GameService(ILogger<GameService> logger)
    {
        activePlayers = new ConcurrentDictionary<string, PlayerStatus>();
        matchMakingQueue = new ConcurrentQueue<Player>();
        this.logger = logger;
    }

    public Player MatchMakeAsync(Player player)
    {
        logger.LogDebug("[{location}]: Trying to matchmake player {playerId}", nameof(GameService), player.PlayerId);

        // if (activePlayers.ContainsKey(player.PlayerId))
        // {
        //     activePlayers.
        //     throw new Exception($"Player is already active with status {}");
        // }
        matchMakingQueue.Enqueue(player);
        logger.LogDebug("[{location}]: Player {playerId} was added to the queue", nameof(GameService), player.PlayerId);
        return player;
    }

    public Game GetGameByGameId(string gameId)
    {
        throw new NotImplementedException();
    }

    public Game GetGameByPlayerId(string playerId)
    {
        throw new NotImplementedException();
    }

    public int MakeMove(string playerId, MoveRequest moveRequest)
    {
        throw new NotImplementedException();
    }
}
