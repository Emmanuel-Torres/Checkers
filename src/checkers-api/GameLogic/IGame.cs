using checkers_api.Models.GameModels;

namespace checkers_api.GameLogic;

public interface IGame
{
    public string Id { get; }
    public string CurrentTurn { get; }
    public IEnumerable<Player> Players { get; }
    public GameState State { get; }
    public IEnumerable<Square> Board { get; }
    public void MakeMove(string playerId, MoveRequest moveRequest);
    public bool IsGameOver();
    public IEnumerable<Location> GetValidMoves(string playerId, Location source);
    public GameResults? GetGameResults();
}