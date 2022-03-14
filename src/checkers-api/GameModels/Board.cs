namespace checkers_api.GameModels;
using System.Collections.Generic;
public class Board : IBoard
{
    private Piece?[,] board;

    public Board()
    {
        //by default initializes board to the official checkers starting positions
        board = new Piece?[8, 8]
        {
            {null,new Piece(false),null,new Piece(false),null,new Piece(false),null,new Piece(false)},
            {new Piece(false),null,new Piece(false),null,new Piece(false),null,new Piece(false),null},
            {null,new Piece(false),null,new Piece(false),null,new Piece(false),null,new Piece(false)},
            {null,null,null,null,null,null,null,null},
            {null,null,null,null,null,null,null,null},
            {new Piece(true),null,new Piece(true),null,new Piece(true),null,new Piece(true),null},
            {null,new Piece(true),null,new Piece(true),null,new Piece(true),null,new Piece(true)},
            {new Piece(true),null,new Piece(true),null,new Piece(true),null,new Piece(true),null}
        };
    }
    public Piece? GetPiece(int row, int column)
    {
        if (IsLocationValid(new Location(row, column)))
        {
            return board[row, column];
        }
        throw new Exception("Location is invalid.");
    }
    public void MakeMove(MoveRequest moveRequest)
    {
        var validMoves = GetValidMoves(moveRequest.Source);
        if (validMoves.Contains(moveRequest.Destination))
        {
            var sourcePiece = GetPiece(moveRequest.Source.Row, moveRequest.Source.Column);
            int rowDifferential = moveRequest.Destination.Row - moveRequest.Source.Row;
            int columnDifferential = moveRequest.Destination.Column - moveRequest.Source.Column;
            if (sourcePiece == null)
            {
                throw new Exception("There is no piece on your original location.");
            }
            if (columnDifferential > 1)
            {
                board[moveRequest.Destination.Row - 1, moveRequest.Destination.Column - 1] = null;
            }
            if (sourcePiece.isBlack && moveRequest.Destination.Row == 0 || !sourcePiece.isBlack && moveRequest.Destination.Row == (board.GetLength(0) - 1))
            {
                sourcePiece.isKing = true;
            }
            board[moveRequest.Source.Row, moveRequest.Source.Column] = null;
            board[moveRequest.Destination.Row, moveRequest.Destination.Column] = sourcePiece;
        }
        else
        {
            throw new Exception("Move is invalid.");
        }
        throw new NotImplementedException();
    }
    public IEnumerable<Location> GetValidMoves(Location originLocation)
    {
        List<Location> validMoves = new List<Location>();
        if (IsLocationValid(originLocation))
        {
            var piece = board[originLocation.Column, originLocation.Row];
            if (piece == null)
            {
                throw new Exception("Location does not have a piece.");
            }
            if (piece.isKing)
            {
                for (int rowDifferential = -1; rowDifferential <= 1; rowDifferential += 2)
                {
                    validMoves.AddRange(GetValidHorizontallyAdjacentLocationsToRow(piece, originLocation, rowDifferential));
                }
            }
            else if (piece.isBlack)
            {
                int rowDifferential = -1;
                validMoves.AddRange(GetValidHorizontallyAdjacentLocationsToRow(piece, originLocation, rowDifferential));
            }
            else if (!piece.isBlack)
            {
                int rowDifferential = 1;
                validMoves.AddRange(GetValidHorizontallyAdjacentLocationsToRow(piece, originLocation, rowDifferential));
            }
        }
        return validMoves;
    }
    private IEnumerable<Location> GetValidHorizontallyAdjacentLocationsToRow(Piece piece, Location location, int rowDifferential)
    {
        var validDiagonalLocations = new List<Location>();
        for (int columnDifferential = -1; columnDifferential <= 1; columnDifferential += 2)
        {
            var diagonalLocation = new Location(location.Row + rowDifferential, location.Column + columnDifferential);
            if (IsLocationValid(diagonalLocation))
            {
                var adjacentPiece = GetPiece(diagonalLocation.Row, diagonalLocation.Column);
                //can move in that direction.
                if (adjacentPiece == null)
                {
                    validDiagonalLocations.Append(diagonalLocation);
                }
                //cant occupy slot occupied by the player.
                else if (adjacentPiece.isBlack == piece.isBlack)
                {
                    continue;
                }
                //check if jump is possible and append it.
                else if (adjacentPiece.isBlack != piece.isBlack)
                {
                    var jumpLocation = new Location(location.Row + 2 * rowDifferential, location.Column + 2 * columnDifferential;
                    if (IsLocationValid(jumpLocation))
                    {
                        validDiagonalLocations.Append(jumpLocation);
                    }
                }

            }
        }
        return validDiagonalLocations;
    }
    private bool IsLocationValid(Location location)
    {
        if (location.Row > board.GetLength(0) - 1 || location.Column > board.GetLength(1) - 1 || location.Row < 0 || location.Column < 0)
            return false;
        return true;
    }
}

public class Piece
{
    public Piece(bool _isBlack)
    {
        isBlack = _isBlack;
        isKing = false;
    }
    public Boolean isBlack;
    public Boolean isKing;
}