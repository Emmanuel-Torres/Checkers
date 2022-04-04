using checkers_api.Models.GameModels;

namespace checkers_api.GameLogic;

public interface IGame
{
    string Id { get; }
    string CurrentTurn { get; }
    IEnumerable<Player> Players { get; }
    GameState State { get; }
    IEnumerable<Square> Board { get; }

    void MakeMove(string playerId, MoveRequest moveRequest);
    bool IsGameOver();
    IEnumerable<Location> GetValidMoves(string playerId, Location source);
    GameResults? GetGameResults();
}