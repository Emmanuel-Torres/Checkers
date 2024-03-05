using checkers_api.Models.GameModels;

namespace checkers_api.Models.Responses;

public class GameInfo
{
    public string RoomId { get; }
    public bool IsGameOver { get => Winner is not null; }
    public Player NextPlayerTurn { get; }
    public Player? Winner { get; }
    public IEnumerable<Piece?> Board { get; }

    public GameInfo(string roomId, Player nextPlayerTurn, IEnumerable<Piece?> board, Player? winner = null)
    {
        RoomId = roomId;
        NextPlayerTurn = nextPlayerTurn;
        Board = board;
        Winner = winner;
    }
}