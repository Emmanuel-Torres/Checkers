using checkers_api.Models.DomainModels;
using checkers_api.Models.GameModels;

namespace checkers_api.GameLogic;

public class Game : IGame
{
    private readonly Id id;
    private readonly Player player1;
    private readonly Player player2;
    private readonly Board board;
    private GameState state;
    private Id currentTurn;

    public Game(Player player1, Player player2)
    {
        this.player1 = player1;
        this.player2 = player2;
        currentTurn = player1.PlayerId;
        board = new Board(player1.PlayerId, player2.PlayerId);
        state = GameState.Ongoing;
        id = new Id();
    }

    public Id Id => id;
    public IEnumerable<Player> Players => new List<Player>() { player1, player2 };
    public GameState State => state;
    public Board Board => board;

    public GameResults? GetGameResults()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Location> GetValidMoves(Id playerId, Location source)
    {
        throw new NotImplementedException();
    }

    public bool IsGameOver()
    {
        throw new NotImplementedException();
    }

    public void MakeMove(Id playerId, MoveRequest moveRequest)
    {
        throw new NotImplementedException();
    }
}