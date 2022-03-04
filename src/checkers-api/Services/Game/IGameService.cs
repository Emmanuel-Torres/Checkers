using checkers_api.GameModels;
namespace checkers_api.Services;

public interface IGameService
{
    string? MatchMakeAsync(Player player);
    int MakeMove(string playerId, MoveRequest moveRequest);
    IGame GetGameByGameId(string gameId);
    IGame GetGameByPlayerId(string playerId);
}