using checkers_api.Models.DomainModels;
using checkers_api.Models.GameModels;

namespace checkers_api.GameLogic;

public interface IGame
{
    Id Id { get; }
    IEnumerable<Player> Players { get; }
    GameState State { get; }
    Board Board { get; }

    void MakeMove(Id playerId, MoveRequest moveRequest);
    bool IsGameOver();
    IEnumerable<Location> GetValidMoves(Id playerId, Location source);
    GameResults? GetGameResults();
}