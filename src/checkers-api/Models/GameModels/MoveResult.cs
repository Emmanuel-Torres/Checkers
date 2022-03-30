namespace checkers_api.Models.GameModels;

public class MoveResult
{
    public string GameId { get; }
    public bool WasMoveSuccessful { get; }
    public bool IsGameOver { get; }
    public IEnumerable<Square> Board { get; }

    public MoveResult(string gameId, bool wasMoveSuccessful, bool isGameOver, IEnumerable<Square> board)
    {
        ArgumentNullException.ThrowIfNull(gameId);
        ArgumentNullException.ThrowIfNull(wasMoveSuccessful);
        ArgumentNullException.ThrowIfNull(Board);
        ArgumentNullException.ThrowIfNull(isGameOver);

        GameId = gameId;
        WasMoveSuccessful = wasMoveSuccessful;
        Board = board;
        IsGameOver = isGameOver;
    }
}