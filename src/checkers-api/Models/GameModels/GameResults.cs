namespace checkers_api.Models.GameModels;

public class GameResults
{
    public string GameId { get; }
    public Player Winner { get; }
    public Player Loser { get; }
    public IEnumerable<Player> Players { get; }

    public GameResults(string gameId, Player winner, Player loser)
    {
        GameId = gameId;
        Winner = winner;
        Loser = loser;
        Players = new List<Player>() { Winner, Loser };
    }
}