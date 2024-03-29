using checkers_api.Models.GameModels;

namespace checkers_api.GameLogic;

public class Game
{
    private const int BLACK_PIECES_HOME_ROW = 7;
    private const int WHITE_PIECES_HOME_ROW = 0;
    private readonly string id;
    private readonly Player player1;
    private readonly Player player2;
    private readonly ILogger<Game> logger;
    private readonly Board board;
    private GameState state;
    private string currentTurn;
    public Game(Player player1, Player player2, ILogger<Game> logger)
    {
        this.player1 = player1;
        this.player2 = player2;
        this.logger = logger;
        currentTurn = player1.PlayerId;
        board = new Board(player1.PlayerId, player2.PlayerId);
        state = GameState.Ongoing;
        id = Guid.NewGuid().ToString();
    }

    public string Id => id;
    public IEnumerable<Player> Players => new List<Player>() { player1, player2 };
    public GameState State => state;
    public IEnumerable<Square> Board => board.Squares;

    public void MakeMove(string playerId, MoveRequest moveRequest)
    {
        if (playerId != currentTurn)
        {
            throw new Exception("It is not this player's turn");
        }

        var source = board.GetSquareByLocation(moveRequest.Source);
        var destination = board.GetSquareByLocation(moveRequest.Destination);

        if (!IsMoveValid(source, destination, playerId))
        {
            throw new Exception("Move was not valid");
        }

        MovePiece(source, destination);
        currentTurn = Players.First(p => p.PlayerId != playerId).PlayerId;

        if (IsGameOver())
        {
            state = GameState.GameOver;
        }
    }

    public GameResults? GetGameResults()
    {
        if (state == GameState.Ongoing)
        {
            return null;
        }

        return new GameResults(Id, Players.First(p => p.PlayerId != currentTurn), Players.First(p => p.PlayerId == currentTurn), Board);
    }

    public IEnumerable<Location> GetValidMoves(string playerId, Location location)
    {
        ArgumentNullException.ThrowIfNull(playerId);
        ArgumentNullException.ThrowIfNull(location);

        var square = board.GetSquareByLocation(location);

        if (currentTurn != playerId)
        {
            throw new Exception("Is not this player's turn");
        }
        if (square.Piece is null)
        {
            throw new Exception("Square is does not have a piece");
        }
        if (square.Piece.OwnerId != playerId)
        {
            throw new Exception("Player does not own this piece");
        }

        var neighborSquares = GetNeighbors(square.Piece, location);
        logger.LogDebug("[{location}]: Found {count} neighbor squares", nameof(Game), neighborSquares.Count());

        var availableLocations = neighborSquares.Where(s => s.Piece is null).Select(x => x.Location);
        logger.LogDebug("[{location}]: Found {count} available locations", nameof(Game), availableLocations.Count());

        var occupiedSquares = neighborSquares.Where(s => s.Piece is not null && s.Piece.OwnerId != playerId);

        if (!occupiedSquares.Any())
        {
            return availableLocations;
        }

        var attackLocations = GetAttackLocations(square, occupiedSquares);
        logger.LogDebug("[{location}]: Found {count} attack locations", nameof(Game), attackLocations.Count());

        return availableLocations.Union(attackLocations);
    }

    public bool IsGameOver()
    {
        var ownedSquares = board.Squares.Where(s => s.Piece is not null && s.Piece.OwnerId == currentTurn);

        if (!ownedSquares.Any())
        {
            return true;
        }

        foreach (var s in ownedSquares)
        {
            if (GetValidMoves(currentTurn, s.Location).Any())
            {
                return false;
            }
        }

        return false;
    }

    private void MovePiece(Square source, Square destination)
    {
        var piece = source.Piece!;
        board.RemovePiece(source.Location);
        board.PlacePiece(destination.Location, piece);

        if (IsMoveAnAttack(source, destination))
        {
            var midLocation = GetMiddleLocation(source.Location, destination.Location);
            board.RemovePiece(midLocation);
        }
        if (CanKingPiece(destination.Location, piece))
        {
            logger.LogDebug("[{location}]: Piece at location ({row}, {column}) can be kinged", nameof(Game), destination.Location.Row, destination.Location.Column);
            board.KingPiece(destination.Location);
        }
    }

    private bool IsMoveValid(Square source, Square destination, string playerId)
    {
        var validMoves = GetValidMoves(playerId, source.Location);
        return validMoves.Any(m => m == destination.Location);
    }

    private bool IsMoveRegularOrAttack(Square source, Square destination)
    {
        return Math.Abs(source.Location.Row - destination.Location.Row) == 1 || IsAttackValid(source, destination);
    }

    private bool IsMoveAnAttack(Square source, Square destination)
    {
        if (Math.Abs(source.Location.Row - destination.Location.Row) == 2 && Math.Abs(source.Location.Column - destination.Location.Column) == 2)
        {
            return true;
        }
        return false;
    }

    private bool IsAttackValid(Square source, Square destination)
    {
        var midLocation = GetMiddleLocation(source.Location, destination.Location);
        var midSquare = board.GetSquareByLocation(midLocation);
        if (!midSquare.IsOccupied)
        {
            return false;
        }
        if (midSquare.Piece?.Color != source.Piece?.Color)
        {
            return true;
        }

        return false;
    }

    private bool CanKingPiece(Location location, Piece piece)
    {
        if (piece.Color == Color.Black)
        {
            return location.Row == WHITE_PIECES_HOME_ROW;
        }

        return location.Row == BLACK_PIECES_HOME_ROW;
    }

    private Location GetMiddleLocation(Location source, Location destination)
    {
        return new Location((source.Row + destination.Row) / 2,
                            (source.Column + destination.Column) / 2);
    }

    private IEnumerable<Square> GetNeighbors(Piece piece, Location location)
    {
        var neighborLocations = new[]
        {
            new Location (location.Row - 1, location.Column + 1),
            new Location (location.Row - 1, location.Column - 1),
            new Location (location.Row + 1, location.Column + 1),
            new Location (location.Row + 1, location.Column - 1)
        };

        if (piece.State == PieceState.King)
        {
            return board.Squares.Where(s => neighborLocations.Contains(s.Location));
        }
        if (piece.Color == Color.Black)
        {
            return board.Squares.Where(s => s.Location.Row < location.Row).Where(s => neighborLocations.Contains(s.Location));
        }

        return board.Squares.Where(s => s.Location.Row > location.Row).Where(t => neighborLocations.Contains(t.Location));
    }

    private IEnumerable<Location> GetAttackLocations(Square source, IEnumerable<Square> occupiedNeighbors)
    {

        var locations = new List<Location>();
        foreach (var n in occupiedNeighbors)
        {
            var square = GetNextSquareInTheDiagonal(source, n);
            if (square is null)
            {
                continue;
            }
            if (square.Piece is null)
            {
                locations.Add(square.Location);
            }
        }

        return locations;
    }

    private Square? GetNextSquareInTheDiagonal(Square source, Square middle)
    {
        var diffCol = middle.Location.Column - source.Location.Column;
        var diffRow = middle.Location.Row - source.Location.Row;
        if (Math.Abs(diffCol) != 1 || Math.Abs(diffRow) != 1)
        {
            throw new Exception("There squares are not next to each other in a diagonal");
        }

        try
        {
            var col = middle.Location.Column + diffCol;
            var row = middle.Location.Row + diffRow;
            return board.GetSquareByLocation(new Location(row, col));
        }
        catch
        {
            return null;
        }
    }
}