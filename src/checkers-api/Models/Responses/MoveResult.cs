using checkers_api.Models.GameModels;

namespace checkers_api.Models.Responses;

public class MoveResult
{
    public string RoomId { get; }
    public bool IsGameOver { get; }
    public Player NextPlayerTurn { get; }
    public IEnumerable<Piece?> Board { get; }

    public MoveResult(string roomId, Player nextPlayerTurn, bool isGameOver, IEnumerable<Piece?> board)
    {
        RoomId = roomId;
        NextPlayerTurn = nextPlayerTurn;
        Board = board;
        IsGameOver = isGameOver;
    }
}