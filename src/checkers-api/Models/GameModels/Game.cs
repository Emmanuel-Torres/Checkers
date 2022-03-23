using checkers_api.Models.DomainModels;

namespace checkers_api.Models.GameModels;

public class Game : IGame
{
    private readonly Id gameId;
    private readonly Player player1; //black pieces
    private readonly Player player2; //white pieces
    private Player turnOwner;
    private Board gameBoard;
    private GameState state;

    public Game(Player player1, Player player2)
    {
        gameId = new Id();
        this.player1 = player1;
        this.player2 = player2;
        turnOwner = player1;
        gameBoard = new Board();
        state = GameState.Ongoing;
    }

    public Id GameId => gameId;
    public IEnumerable<Player> Players => new List<Player>() { player1, player2 };
    public GameState State => state;
    public Board Board => gameBoard;

    public GameResults? GetGameResults()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Location> GetValidMoves(Id playerId, Location source)
    {
        if (isPlayerPieceOwner(playerId, source))
        {
            return gameBoard.GetValidMoves(source);
        }
        else throw new Exception("Player does not own this piece.");
    }

    public bool IsGameOver()
    {
        if (gameBoard.GetNumberOfPiecesByColor(true) == 0 || gameBoard.GetNumberOfPiecesByColor(false) == 0)
        {
            return true;
        }
        else return false;
    }

    public void MakeMove(Id playerId, MoveRequest moveRequest)
    {
        //TODO ALLOW FOR CONSECUTIVE JUMPS
        if (turnOwner.PlayerId == playerId)
        {
            if (isPlayerPieceOwner(playerId, moveRequest.Source))
            {
                gameBoard.MakeMove(moveRequest);
                if (IsGameOver())
                {
                    state = GameState.GameOver;
                }
                else
                {
                    if (playerId == player1.PlayerId) turnOwner = player2;
                    else turnOwner = player1;
                }
            }
            else throw new Exception("Player does not own this piece");
        }
        else throw new Exception("Is not the player's turn");
    }
    private bool isPlayerPieceOwner(Id playerId, Location pieceLocation)
    {
        var piece = gameBoard.GetPiece(pieceLocation);
        if (piece == null) throw new Exception("There is no piece in this location.");
        if ((piece.isBlack && playerId == player1.PlayerId) || (!piece.isBlack && playerId == player2.PlayerId))
        {
            return true;
        }
        else return false;
    }
}