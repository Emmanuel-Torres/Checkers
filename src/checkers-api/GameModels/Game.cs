namespace checkers_api.GameModels;

public class Game : IGame
{
    private readonly Player player1; //black pieces
    private readonly Player player2; //white pieces

    public string GameId { get => throw new NotImplementedException(); }

    public IEnumerable<Player> Players => new List<Player>() { player1, player2 };
    public Board gameBoard { get; set; }

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

        var piece = gameBoard.GetPiece(source.Column, source.Row);
        if (piece == null) throw new Exception("There is no piece in this location.");
        if ((piece.isBlack && playerId == player1.PlayerId) || (!piece.isBlack && playerId == player2.PlayerId))
        {
            return gameBoard.GetValidMoves(source);
        }
        else throw new Exception("Player doesnt own this piece.");
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