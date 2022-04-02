using checkers_api.Models.GameModels;

namespace checkers_api.Services;

public interface IMatchmakingService
{
    Task MatchMakeAsync(Player player);
    bool CancelMatchMaking(string playerId);
    Task ConfigureQueue(Func<Player, Player, Task> startGame);
}