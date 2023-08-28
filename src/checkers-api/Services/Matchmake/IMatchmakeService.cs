using checkers_api.Models.GameModels;

namespace checkers_api.Services;

public interface IMatchmakingService
{
    public Task MatchMakeAsync(Player player);
    public bool CancelMatchMaking(string playerId);
}