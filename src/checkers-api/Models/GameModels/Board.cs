namespace checkers_api.Models.GameModels;
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
    public int GetNumberOfPiecesByColor(bool colorIsBlack)
    {
        var count = 0;
        foreach (Piece? piece in board)
        {
            if (piece != null && piece.isBlack == colorIsBlack)
            {
                count++;
            }
        }
        return count;
    }

    public Piece?[,] GetBoard()
    {
        return board;
    }
    public Piece? GetPiece(Location location)
    {
        try
        {
            TryLocation(location);
            return board[location.Row, location.Column];
        }
        catch
        {
            throw;
        }
    }
    public void MakeMove(MoveRequest moveRequest)
    {
        var validMoves = GetValidMoves(moveRequest.Source);
        if (validMoves.Contains(moveRequest.Destination))
        {
            var sourcePiece = GetPiece(moveRequest.Source);
            int rowDifferential = moveRequest.Destination.Row - moveRequest.Source.Row;
            int columnDifferential = moveRequest.Destination.Column - moveRequest.Source.Column;
            if (sourcePiece == null)
            {
                throw new Exception("There is no piece on your original location.");
            }
            if (columnDifferential > 1 && rowDifferential > 1)
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
    }
    public IEnumerable<Location> GetValidMoves(Location originLocation)
    {
        try
        {
            List<Location> validMoves = new List<Location>();
            TryLocation(originLocation);
            var piece = board[originLocation.Column, originLocation.Row];
            if (piece == null)
            {
                throw new Exception("Location does not contain a piece.");
            }
            if (piece.isKing)
            {
                //move up and down
                validMoves.AddRange(GetValidHorizontalLocatonsInDirection(piece, originLocation, -1));
                validMoves.AddRange(GetValidHorizontalLocatonsInDirection(piece, originLocation, 1));
            }
            else if (piece.isBlack)
            {
                //move up
                validMoves.AddRange(GetValidHorizontalLocatonsInDirection(piece, originLocation, -1));
            }
            else if (!piece.isBlack)
            {
                //move down
                validMoves.AddRange(GetValidHorizontalLocatonsInDirection(piece, originLocation, 1));
            }
            return validMoves;
        }
        catch
        {
            throw;
        }
    }
    private IEnumerable<Location> GetValidHorizontalLocatonsInDirection(Piece piece, Location location, int rowDirection)
    {
        var validDiagonalLocations = new List<Location>();
        try
        {
            for (int columnDifferential = -1; columnDifferential <= 1; columnDifferential += 2)
            {
                var diagonalLocation = new Location(location.Row + rowDirection, location.Column + columnDifferential);
                TryLocation(diagonalLocation);
                var adjacentPiece = GetPiece(diagonalLocation);
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
                    var jumpLocation = new Location(location.Row + 2 * rowDirection, location.Column + 2 * columnDifferential);
                    TryLocation(jumpLocation);
                    validDiagonalLocations.Append(jumpLocation);
                }
            }
            return validDiagonalLocations;
        }
        catch
        {
            throw;
        }
    }
    private void TryLocation(Location location)
    {
        if (location.Row > board.GetLength(0) - 1 || location.Column > board.GetLength(1) - 1 || location.Row < 0 || location.Column < 0)
            throw new Exception("Location is invalid");
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