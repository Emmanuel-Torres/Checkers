namespace checkers_api.GameModels;

public interface IGame
{
    IEnumerable<Player> Players { get; }
    string GameId { get; }
    void MakeMove(string playerId, MoveRequest moveRequest);
    bool IsGameOver();
    IEnumerable<Location> GetValidMoves(string playerId, Location source);
    GameResults? GetGameResults();
}