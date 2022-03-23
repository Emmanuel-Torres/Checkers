using checkers_api.Models.DomainModels;
using checkers_api.Models.GameModels;
namespace checkers_api.Services;

public interface IGameService
{
    Id StartGame(Player player1, Player player2);
    bool TryMakeMove(Id playerId, MoveRequest moveRequest);
    IGame? GetGameByGameId(Id gameId);
    IGame? GetGameByPlayerId(Id playerId);
    void QuitGame(Id playerId);
}