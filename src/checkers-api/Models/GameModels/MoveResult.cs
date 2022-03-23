namespace checkers_api.Models.GameModels;

public class MoveResult
{
    public bool WasMoveSuccessful { get; private set; }
    public bool IsGameOver { get; private set; }
    public Board Board { get; private set; }

    public MoveResult(bool wasMoveSuccessful, bool isGameOver, Board board)
    {
        ArgumentNullException.ThrowIfNull(wasMoveSuccessful);
        ArgumentNullException.ThrowIfNull(Board);
        ArgumentNullException.ThrowIfNull(isGameOver);

        WasMoveSuccessful = wasMoveSuccessful;
        Board = board;
        IsGameOver = isGameOver;
    }
}