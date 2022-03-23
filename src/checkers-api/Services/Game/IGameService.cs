using checkers_api.Models.DomainModels;
using checkers_api.Models.GameModels;
namespace checkers_api.Services;

public interface IGameService
{
    Id StartGame(Player player1, Player player2);
    GameState TryMakeMove(Id playerId, MoveRequest moveRequest);
    IGame? GetGameByGameId(Id gameId);
    IGame? GetGameByPlayerId(Id playerId);
    void RemovePlayerFromGame(Id playerId);
}