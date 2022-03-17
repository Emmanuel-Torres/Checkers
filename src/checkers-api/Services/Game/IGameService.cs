using checkers_api.DomainModels;
using checkers_api.GameModels;
namespace checkers_api.Services;

public interface IGameService
{
    Id? MatchMakeAsync(Player player);
    bool TryMakeMove(Id playerId, MoveRequest moveRequest);
    IGame? GetGameByGameId(Id gameId);
    IGame? GetGameByPlayerId(Id playerId);
}