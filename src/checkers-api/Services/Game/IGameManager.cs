using checkers_api.Models.GameModels;
using checkers_api.Models.Requests;
using checkers_api.Models.Responses;

namespace checkers_api.Services.GameManager;

public interface IGameManager
{
    public GameInfo CreateGame(Player player1, Player player2);
    public GameInfo MakeMove(string playerId, MoveRequest moveRequest);

    // public string StartGame(Player player1, Player player2);
    // public Game? GetGameByGameId(string gameId);
    // public Game? GetGameByPlayerId(string playerId);
    // public GameResults TerminateGame(string gameId);
    // public GameResults QuitGame(string playerId);
    // public IEnumerable<Location> GetValidMoves(string playerId, Location location);
}