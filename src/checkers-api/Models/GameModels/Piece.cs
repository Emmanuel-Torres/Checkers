namespace checkers_api.Models.GameModels;

public class Piece
{
    public string OwnerId { get; }
    public PieceState State { get; private set; }

    public Piece(string ownerId)
    {
        ArgumentNullException.ThrowIfNull(ownerId);

        OwnerId = ownerId;
        State = PieceState.Regular;
    }

    public void KingPiece()
    {
        State = PieceState.King;
    }
}