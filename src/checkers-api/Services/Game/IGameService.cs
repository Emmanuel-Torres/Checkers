using checkers_api.GameModels;
namespace checkers_api.Services;

public interface IGameService
{
    Player MatchMakeAsync(Player player);
    int MakeMove(string playerId, MoveRequest moveRequest);
    Game GetGameByGameId(string gameId);
    Game GetGameByPlayerId(string playerId);
}