using checkers_api.Models.DomainModels;

namespace checkers_api.Models.GameModels;

public interface IGame
{
    Id GameId { get; }
    IEnumerable<Player> Players { get; }
    GameState State { get; }
    Board Board { get; }

    void MakeMove(Id playerId, MoveRequest moveRequest);
    bool IsGameOver();
    IEnumerable<Location> GetValidMoves(Id playerId, Location source);
    GameResults? GetGameResults();
}