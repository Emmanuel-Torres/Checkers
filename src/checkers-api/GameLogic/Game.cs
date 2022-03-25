using checkers_api.Models.DomainModels;
using checkers_api.Models.GameModels;
using Microsoft.Azure.Amqp.Framing;

namespace checkers_api.GameLogic;

public class Game : IGame
{
    private const int BLACK_PIECES_HOME_ROW = 7;
    private const int WHITE_PIECES_HOME_ROW = 0;
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
        var blackPiece = new Piece(Color.Black, player1.PlayerId);
        var whitePiece = new Piece(Color.White, player2.PlayerId);
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
            throw new Exception("It is not this player's turn");
        }

        try
        {
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
        catch
        {
            throw;
        }
    }

    public GameResults? GetGameResults()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Location> GetValidMoves(Id playerId, Location location)
    {
        ArgumentNullException.ThrowIfNull(playerId);
        ArgumentNullException.ThrowIfNull(location);

        var square = Board.GetSquareByLocation(location);

        if (square.Piece is null ||
            square.Piece.OwnerId != playerId)
        {
            return new List<Location>();
        }

        var neighborSquares = GetNeighbors(square.Piece.Color, location);
        var availableLocations = neighborSquares.Where(s => s.Piece is null).Select(x => x.Location);
        var occupiedSquares = neighborSquares.Where(s => s.Piece is not null && s.Piece.OwnerId != playerId);
        var nextLocations = new List<Location>();

        if (occupiedSquares.Any())
        {
            foreach (var s in occupiedSquares)
            {
                var nextSquare = GetNeighbors(square.Piece.Color, s.Location)
                                    .Where(x => x.IsOccupied == false)
                                    .FirstOrDefault(x => x.Location.Column != square.Location.Column);

                if (nextSquare != null)
                {
                    nextLocations.Add(nextSquare.Location);
                }
            }
        }

        return availableLocations.Union(nextLocations);
    }

    public bool IsGameOver()
    {
        var ownedSquares = Board.Squares.Where(s => s.Piece is not null && s.Piece.OwnerId == currentTurn);

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
        try
        {
            var piece = source.Piece!;
            Board.RemovePiece(source.Location);
            Board.PlacePiece(destination.Location, piece);

            if (IsMoveAnAttack(source, destination))
            {
                var midLocation = GetMiddleLocation(source.Location, destination.Location);
                Board.RemovePiece(midLocation);
            }
            if (CanKingPiece(destination.Location, piece))
            {
                Board.KingPiece(destination.Location);
            }
        }
        catch
        {
            throw;
        }
    }

    private bool IsMoveValid(Square source, Square destination, Id playerId)
    {
        if (source.Piece is not null &&
            source.Piece.OwnerId == playerId &&
            IsPieceGoingForward(source, destination) &&
            destination.Piece is null &&
            Math.Abs(source.Location.Row - destination.Location.Row) <= 2 &&
            Math.Abs(source.Location.Column - destination.Location.Column) <= 2 &&
            IsMoveRegularOrAttack(source, destination))
        {
            return true;
        }

        return false;
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

    private bool IsPieceGoingForward(Square source, Square destination)
    {
        var distance = destination.Location.Row - source.Location.Row;

        if (source.Piece?.State == PieceState.King)
        {
            return false;
        }
        if (source.Piece?.Color == Color.Black)
        {
            return distance > 0;
        }
        if (source.Piece?.Color == Color.White)
        {
            return distance < 0;
        }

        return false;
    }

    private bool IsAttackValid(Square source, Square destination)
    {
        var midLocation = GetMiddleLocation(source.Location, destination.Location);
        var midSquare = Board.GetSquareByLocation(midLocation);
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

    private IEnumerable<Square> GetNeighbors(Color color, Location location)
    {
        var neighborLocations = new[]
        {
            new Location (location.Row - 1, location.Column + 1 ),
            new Location (location.Row - 1, location.Column - 1 ),
            new Location (location.Row + 1, location.Column + 1),
            new Location (location.Row + 1, location.Column - 1)
        };

        if (board.GetSquareByLocation(location).Piece?.State == PieceState.King)
        {
            return board.Squares.Where(s => neighborLocations.Contains(s.Location));
        }

        if (color == Color.Black)
        {
            return board.Squares.Where(s => s.Location.Row < location.Row).Where(s => neighborLocations.Contains(s.Location));
        }

        return board.Squares.Where(s => s.Location.Row > location.Row).Where(t => neighborLocations.Contains(t.Location));
    }
}