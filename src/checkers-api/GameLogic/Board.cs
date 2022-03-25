using checkers_api.Models.DomainModels;
using checkers_api.Models.GameModels;

namespace checkers_api.GameLogic;

public class Board
{
    private readonly List<Square> squares;
    public IEnumerable<Square> Squares => squares;

    public Board(Piece blackPiece, Piece whitePiece)
    {
        squares = new List<Square>();
        GenerateBoard();
        GeneratePieces(blackPiece, whitePiece);
    }

    public void PlacePiece(Location location, Piece piece)
    {
        var index = GetSquareIndex(location);

        if (index < 0)
        {
            throw new Exception("Location was not found");
        }
        if (squares[index].IsOccupied)
        {
            throw new Exception("This location is already occupied");
        }

        squares[index].Piece = piece;
    }

    public void RemovePiece(Location location)
    {
        var index = GetSquareIndex(location);

        if (index < 0)
        {
            throw new Exception("Location was not found");
        }
        if (!squares[index].IsOccupied)
        {
            throw new Exception("This location has no piece to remove");
        }

        squares[index].Piece = null;
    }

    public Square GetSquareByLocation(Location location)
    {
        return squares.First(l => l.Location.Column == location.Column && l.Location.Row == location.Row);
    }

    public void KingPiece(Location location)
    {
        var index = GetSquareIndex(location);

        if (index < 0)
        {
            throw new Exception("Location was not found");
        }
        if (!squares[index].IsOccupied)
        {
            throw new Exception("This location has no piece to king");
        }

        squares[index].Piece!.KingPiece();
    }

    private int GetSquareIndex(Location location)
    {
        return squares.FindIndex(s => s.Location.Row == location.Row && s.Location.Column == location.Column);
    }

    private void GenerateBoard()
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                var color = Color.White;
                if (row % 2 == col % 2)
                {
                    color = Color.Black;
                }
                squares.Add(new Square(new Location(row, col), color));
            }
        }
    }

    private void GeneratePieces(Piece blackPiece, Piece whitePiece)
    {
        for (int i = 0; i < 12; i++)
        {
            try
            {
                var square = squares.First(s => s.Color == Color.Black && s.IsOccupied == false);
                PlacePiece(square.Location, whitePiece);
            }
            catch
            {
                throw;
            }
        }

        for (int i = 0; i < 12; i++)
        {
            try
            {
                var square = squares.First(s => s.Color == Color.Black && s.IsOccupied == false && s.Location.Row >= 5);
                PlacePiece(square.Location, blackPiece);
            }
            catch
            {
                throw;
            }
        }
    }
}