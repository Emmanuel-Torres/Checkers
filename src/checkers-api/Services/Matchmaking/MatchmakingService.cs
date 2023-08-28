using System.Collections.Concurrent;
using checkers_api.Helpers;
using checkers_api.Models.Events;
using checkers_api.Models.GameModels;

namespace checkers_api.Services.Matchmaking;

public class MatchmakingService : IMatchmakingService
{
    private readonly ILogger<MatchmakingService> _logger;
    private readonly ConcurrentQueue<Player> _waitingQueue;

    public event Func<object, PlayersMatchedEventArgs, Task>? PlayersMatched;
    
    public MatchmakingService(ILogger<MatchmakingService> logger)
    {
        _logger = logger;
        _waitingQueue = new();
    }

    public Task StartMatchmakingAsync(Player player)
    {
        throw new NotImplementedException();
    }

    public bool CancelMatchmaking(string playerId)
    {
        throw new NotImplementedException();
    }

    private async Task OnPlayersMatched(PlayersMatchedEventArgs args)
    {
        if (PlayersMatched is not null)
            await PlayersMatched.InvokeAsync(this, args);
    }
}