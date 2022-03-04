namespace checkers_api.GameModels;

public class Game : IGame
{
    private readonly Player player1;
    private readonly Player player2;

    public string GameId { get => throw new NotImplementedException(); }

    public IEnumerable<Player> Players => new List<Player>() { player1, player2 };

    public Game(Player player1, Player player2)
    {
        this.player1 = player1;
        this.player2 = player2;
    }

    public GameResults? GetGameResults()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Location> GetValidMoves(string playerId, Location source)
    {
        throw new NotImplementedException();
    }

    public bool IsGameOver()
    {
        throw new NotImplementedException();
    }

    public void MakeMove(string playerId, MoveRequest moveRequest)
    {
        throw new NotImplementedException();
    }
}