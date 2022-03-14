namespace checkers_api.GameModels;

public class Game : IGame
{
    public string GameId { get; set; }
    private readonly Player player1; //black pieces
    private readonly Player player2; //white pieces
    private Player turnOwner;
    private Board gameBoard;
    private GameState gameState;

    public Game(string GameId, Player player1, Player player2)
    {
        this.GameId = GameId;
        this.player1 = player1;
        this.player2 = player2;
        turnOwner = player1;
        gameBoard = new Board();
        gameState = GameState.InGame;
    }

    public GameResults? GetGameResults()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Location> GetValidMoves(string playerId, Location source)
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

    public void MakeMove(string playerId, MoveRequest moveRequest)
    {
        //TODO ALLOW FOR CONSECUTIVE JUMPS
        if (turnOwner.PlayerId == playerId)
        {
            if (isPlayerPieceOwner(playerId, moveRequest.Source))
            {
                gameBoard.MakeMove(moveRequest);
                if (IsGameOver())
                {
                    gameState = GameState.GameOver;
                }
                else
                {
                    if (playerId == player1.PlayerId) turnOwner = player2;
                    else turnOwner = player1;
                }
            }
            else throw new Exception("Player doesnt own this piece.");
        }
        else throw new Exception("You can't execute this action at the moment.");
    }
    private bool isPlayerPieceOwner(string playerId, Location pieceLocation)
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

enum GameState
{
    InGame,
    GameOver
}