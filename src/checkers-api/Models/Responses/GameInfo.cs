using checkers_api.Models.GameModels;

namespace checkers_api.Models.Responses;

public class GameInfo
{
    public string RoomId { get; }
    public bool IsGameOver { get; }
    public Player NextPlayerTurn { get; }
    public Player? Winner { get; }
    public List<List<Piece?>> Board { get; }

    public GameInfo(string roomId, Player nextPlayerTurn, List<List<Piece?>> board, Player? winner = null)
    {
        RoomId = roomId;
        NextPlayerTurn = nextPlayerTurn;
        Board = board;
        Winner = winner;
        IsGameOver = winner is not null;
    }
}