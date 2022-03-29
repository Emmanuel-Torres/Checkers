using checkers_api.Models.GameModels;
using checkers_api.GameLogic;

namespace checkers_api.Services;

public interface IGameService
{
    string StartGame(Player player1, Player player2);
    MoveResult MakeMove(string playerId, MoveRequest moveRequest);
    IGame? GetGameByGameId(string gameId);
    IGame? GetGameByPlayerId(string playerId);
    GameResults GetGameResults(string gameId);
    GameResults QuitGame(string playerId);
}