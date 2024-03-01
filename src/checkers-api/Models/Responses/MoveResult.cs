using checkers_api.Models.GameModels;

namespace checkers_api.Models.Responses;

public class MoveResponse
{
    public string GameId { get; }
    public bool WasMoveSuccessful { get; }
    public bool IsGameOver { get; }
    public IEnumerable<Piece?> Board { get; }

    public MoveResponse(string gameId, bool wasMoveSuccessful, bool isGameOver, IEnumerable<Piece?> board)
    {
        GameId = gameId;
        WasMoveSuccessful = wasMoveSuccessful;
        Board = board;
        IsGameOver = isGameOver;
    }
}