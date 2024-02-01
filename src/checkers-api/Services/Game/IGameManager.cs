using checkers_api.Models.GameModels;
using checkers_api.Models.Requests;
using checkers_api.GameLogic;

namespace checkers_api.Services.GameManager;

public interface IGameManager
{
    public string StartGame(Player player1, Player player2);
    public MoveResult MakeMove(string playerId, MoveRequest moveRequest);
    public Game? GetGameByGameId(string gameId);
    public Game? GetGameByPlayerId(string playerId);
    // public GameResults TerminateGame(string gameId);
    // public GameResults QuitGame(string playerId);
    public IEnumerable<Location> GetValidMoves(string playerId, Location location);
}