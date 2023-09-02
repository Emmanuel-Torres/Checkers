using System.Collections.Concurrent;
using checkers_api.Helpers;
using checkers_api.Models.GameModels;

namespace checkers_api.Services.Matchmaking;

public class MatchmakingService : IMatchmakingService
{
    private readonly ILogger<MatchmakingService> _logger;
    private readonly ConcurrentQueue<Player> _waitingQueue;

    private event Func<object, EventArgs, Task> _queueUpdated;

    public MatchmakingService(ILogger<MatchmakingService> logger)
    {
        _logger = logger;
        _waitingQueue = new();
        _queueUpdated += OnQueueUpdated;
    }

    public Func<Player, Player, Task>? OnPlayersMatched { get; set; }

    public async Task StartMatchmakingAsync(Player player)
    {
        _logger.LogDebug("[{location}]: Adding player {playerId} to waiting queue", nameof(MatchmakingService), player.PlayerId);
        _waitingQueue.Enqueue(player);
        await _queueUpdated.InvokeAsync(this, new EventArgs());
    }

    public bool CancelMatchmaking(string playerId)
    {
        throw new NotImplementedException();
    }

    private async Task OnQueueUpdated(object? sender, EventArgs e)
    {
        _logger.LogDebug("[{location}]: Queue has been updated", nameof(MatchmakingService));

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

            if (OnPlayersMatched is not null)
                await OnPlayersMatched(p1, p2);
            else
                throw new InvalidOperationException($"{nameof(OnPlayersMatched)} has not been set");
        }
    }
}