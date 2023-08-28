using System.Collections.Concurrent;
using checkers_api.Models.GameModels;

namespace checkers_api.Services;

public class MatchmakingService : IMatchmakingService
{
    private readonly ILogger<MatchmakingService> _logger;
    private readonly ConcurrentQueue<Player> _matchMakingQueue;

    public MatchmakingService(ILogger<MatchmakingService> logger)
    {
        _logger = logger;
        _matchMakingQueue = new();
    }

    public Task MatchMakeAsync(Player player)
    {
        throw new NotImplementedException();
    }

    public bool CancelMatchMaking(string playerId)
    {
        throw new NotImplementedException();
    }
}