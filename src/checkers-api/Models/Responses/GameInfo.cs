using checkers_api.Models.GameModels;

namespace checkers_api.Models.Responses;

public class GameInfo
{
    public string GameId { get; set; }
    public IEnumerable<Piece?> Board { get; set; }
    public string CurrentTurnId { get; set; }

    public GameInfo(string gameId, IEnumerable<Piece?> board, string currentTurnId)
    {
        GameId = gameId;
        Board = board;
        CurrentTurnId = currentTurnId;
    }
}