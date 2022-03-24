using checkers_api.Models.DomainModels;
using checkers_api.Models.GameModels;

namespace checkers_api.GameLogic;

public class Game : IGame
{
    private readonly Id id;
    private readonly Player player1;
    private readonly Player player2;
    private readonly Piece blackPiece;
    private readonly Piece whitePiece;
    private readonly Board board;
    private GameState state;
    private Id currentTurn;
    public Game(Player player1, Player player2)
    {
        this.player1 = player1;
        this.player2 = player2;
        currentTurn = player1.PlayerId;
        blackPiece = new Piece(Color.Black, player1.PlayerId);
        whitePiece = new Piece(Color.White, player2.PlayerId);
        board = new Board(blackPiece, whitePiece);
        state = GameState.Ongoing;
        id = new Id();
    }

    public Id Id => id;
    public Id CurrentTurn => currentTurn;
    public IEnumerable<Player> Players => new List<Player>() { player1, player2 };
    public GameState State => state;
    public Board Board => board;

    public void MakeMove(Id playerId, MoveRequest moveRequest)
    {
        if (playerId != currentTurn)
        {
            throw new Exception("It is not this players turn");
        }
        if (!IsMoveValid(moveRequest.Source, moveRequest.Destination))
        {
            throw new Exception("Move was not valid");
        }

        //Logic to make move happen

        currentTurn = Players.First(p => p.PlayerId != playerId).PlayerId;
    }

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

    private bool IsMoveValid(Location source, Location destination)
    {
        return false;
    }
}