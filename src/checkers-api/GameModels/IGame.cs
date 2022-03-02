namespace checkers_api.GameModels;

public interface IGame
{
    void MakeMove(string playerId, MoveRequest moveRequest);
    bool IsGameOver();
    IEnumerable<Location> GetValidMoves(string playerId, Location source);
    GameResults? GetGameResults();
}