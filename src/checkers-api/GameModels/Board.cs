namespace checkers_api.GameModels;

public class Board : IBoard
{
    private Piece?[,] board;

    public Board()
    {
        board = new Piece?[8, 8];
    }

    public void MakeMove(MoveRequest moveRequest)
    {
        throw new NotImplementedException();
    }
    public IEnumerable<Location> GetValidMoves(Location location)
    {
        IEnumerable<Location> validMoves = new List<Location>();
        if (IsLocationValid(location))
        {
            var piece = board[location.Column, location.Row];
            if (piece == null)
            {
                throw new Exception("Location does not have a piece.");
            }
            if (piece.isBlack)
            {
                //moves diagonally up
                if (location.Row - 1 < 0)
                {
                    //check if space is on board and if slot is empty or not occupied by a player piece
                    if (!(location.Column + 1 > board.GetLength(1)) && GetPiece(location.Row - 1, location.Column + 1).isBlack != piece.isBlack)
                        validMoves.Append(new Location(location.Row - 1, location.Column + 1));
                    if (!(location.Column - 1 > board.GetLength(1)) && GetPiece(location.Row - 1, location.Column - 1).isBlack != piece.isBlack)
                        validMoves.Append(new Location(location.Row - 1, location.Column - 1));

                }
                //king is able to move down
                if (piece.isKing)
                {
                    if (location.Row + 1 < board.GetLength(0))
                    {
                        //check if space is on board and if slot is empty or not occupied by a player piece
                        if (!(location.Column + 1 > board.GetLength(1)) && GetPiece(location.Row + 1, location.Column + 1).isBlack != piece.isBlack)
                            validMoves.Append(new Location(location.Row + 1, location.Column + 1));
                        if (!(location.Column - 1 > board.GetLength(1)) && GetPiece(location.Row - 1, location.Column - 1).isBlack != piece.isBlack)
                            validMoves.Append(new Location(location.Row + 1, location.Column - 1));
                    }
                }
            }
            else
            {
                if (location.Row + 1 < board.GetLength(0))
                {
                    //check if space is on board and if slot is empty or not occupied by a player piece
                    if (!(location.Column + 1 > board.GetLength(1)) && GetPiece(location.Row + 1, location.Column + 1).isBlack != piece.isBlack)
                        validMoves.Append(new Location(location.Row + 1, location.Column + 1));
                    if (!(location.Column - 1 > board.GetLength(1)) && GetPiece(location.Row - 1, location.Column - 1).isBlack != piece.isBlack)
                        validMoves.Append(new Location(location.Row + 1, location.Column - 1));
                }
                if (piece.isKing)
                {
                    if (location.Row - 1 < 0)
                    {
                        //check if space is on board and if slot is empty or not occupied by a player piece
                        if (!(location.Column + 1 > board.GetLength(1)) && GetPiece(location.Row - 1, location.Column + 1).isBlack != piece.isBlack)
                            validMoves.Append(new Location(location.Row - 1, location.Column + 1));
                        if (!(location.Column - 1 > board.GetLength(1)) && GetPiece(location.Row - 1, location.Column - 1).isBlack != piece.isBlack)
                            validMoves.Append(new Location(location.Row - 1, location.Column - 1));

                    }
                }
            }


        }
        throw new NotImplementedException();
    }
    public Piece GetPiece(int row, int column)
    {
        throw new NotImplementedException();
    }
    private bool IsLocationValid(Location location)
    {
        if (location.Row > board.GetLength(0) || location.Column > board.GetLength(1)) return false;
        return true;
    }
}

public class Piece
{
    public Boolean isBlack;
    public Boolean isKing;
}