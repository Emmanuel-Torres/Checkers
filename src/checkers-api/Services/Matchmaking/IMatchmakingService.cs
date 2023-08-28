using checkers_api.Models.Events;
using checkers_api.Models.GameModels;

namespace checkers_api.Services.Matchmaking;

public interface IMatchmakingService
{
    public event Func<object, PlayersMatchedEventArgs, Task>? PlayersMatched;
    public Task StartMatchmakingAsync(Player player);
    public bool CancelMatchmaking(string playerId);
}

