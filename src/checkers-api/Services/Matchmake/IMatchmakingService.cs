using checkers_api.Models.GameModels;

namespace checkers_api.Services;

public interface IMatchmakingService
{
    public Task StartMatchmakingAsync(Player player);
    public bool CancelMatchmaking(string playerId);
}