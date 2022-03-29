namespace checkers_api.Models.GameModels;

public class GameResults
{
    public int? Id { get; set; }
    public string GameId { get; set; }
    public Player Winner { get; set; }
    public Player Loser { get; set; }
    public IEnumerable<Player> Players { get; }

    public GameResults(string gameId, Player winner, Player loser, int? id = null)
    {
        GameId = gameId;
        Winner = winner;
        Loser = loser;
        Id = id;
        Players = new List<Player>() { Winner, Loser };
    }
}