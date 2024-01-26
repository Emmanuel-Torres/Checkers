namespace checkers_api.Models.GameModels;

public class MoveResult
{
    public string GameId { get; }
    public bool WasMoveSuccessful { get; }
    public bool IsGameOver { get; }
    public IEnumerable<Piece?> Board { get; }

    public MoveResult(string gameId, bool wasMoveSuccessful, bool isGameOver, IEnumerable<Piece?> board)
    {
        ArgumentNullException.ThrowIfNull(gameId);
        ArgumentNullException.ThrowIfNull(wasMoveSuccessful);
        ArgumentNullException.ThrowIfNull(isGameOver);
        ArgumentNullException.ThrowIfNull(board);

        GameId = gameId;
        WasMoveSuccessful = wasMoveSuccessful;
        Board = board;
        IsGameOver = isGameOver;
    }
}