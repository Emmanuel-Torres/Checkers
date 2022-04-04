namespace checkers_api.Models.GameModels;

public class GameResults
{
    public string GameId { get; }
    public Player Winner { get; }
    public Player Loser { get; }
    public IEnumerable<Player> Players { get; }
    public IEnumerable<Square> Board { get; }

    public GameResults(string gameId, Player winner, Player loser, IEnumerable<Square> board)
    {
        GameId = gameId;
        Winner = winner;
        Loser = loser;
        Board = board;
        Players = new List<Player>() { Winner, Loser };
    }
}