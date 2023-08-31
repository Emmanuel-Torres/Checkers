using checkers_api.Models.GameModels;

namespace checkers_api.Services.Matchmaking;

public interface IMatchmakingService
{
    public Func<Player, Player, Task>? OnPlayersMatched { get; set; }
    public Task StartMatchmakingAsync(Player player);
    public bool CancelMatchmaking(string playerId);
}

