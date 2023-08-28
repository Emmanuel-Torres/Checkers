using System.Collections.Concurrent;
using checkers_api.Helpers;
using checkers_api.Models.Events;
using checkers_api.Models.GameModels;

namespace checkers_api.Services.Matchmaking;

public class MatchmakingService : IMatchmakingService
{
    private readonly ILogger<MatchmakingService> _logger;
    private readonly ConcurrentQueue<Player> _waitingQueue;
    private event Func<object, EventArgs, Task> _queueUpdated;

    public event Func<object, PlayersMatchedEventArgs, Task>? PlayersMatched;
    
    public MatchmakingService(ILogger<MatchmakingService> logger)
    {
        _logger = logger;
        _waitingQueue = new();

        _queueUpdated += OnQueueUpdated;
    }

    public async Task StartMatchmakingAsync(Player player)
    {
        _waitingQueue.Enqueue(player);
        await _queueUpdated.InvokeAsync(this, new EventArgs());
    }

    public bool CancelMatchmaking(string playerId)
    {
        throw new NotImplementedException();
    }

    private async Task OnQueueUpdated(object? sender, EventArgs e)
    {
        while (_waitingQueue.Count > 1)
        {
            if (!_waitingQueue.TryDequeue(out var p1))
            {
                break;
            }
            if (!_waitingQueue.TryDequeue(out var p2))
            {
                _waitingQueue.Enqueue(p1);
                break;
            }
            await OnPlayersMatched(new PlayersMatchedEventArgs(p1, p2));
        }
    }

    private async Task OnPlayersMatched(PlayersMatchedEventArgs args)
    {
        if (PlayersMatched is not null)
            await PlayersMatched.InvokeAsync(this, args);
    }
}