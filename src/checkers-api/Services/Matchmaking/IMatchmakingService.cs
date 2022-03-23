using checkers_api.Models.DomainModels;
using checkers_api.Models.GameModels;

namespace checkers_api.Services;

public interface IMatchmakingService
{
    Task MatchMakeAsync(Player player);
    bool CancelMatchMaking(Id playerId);
}