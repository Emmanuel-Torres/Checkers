namespace checkers_api.Models.GameModels;

public class Piece
{
    public Color Color { get; }
    public string OwnerId { get; }
    public PieceState State { get; private set; }

    public Piece(Color color, string ownerId)
    {
        Color = color;
        OwnerId = ownerId;
        State = PieceState.Regular;
    }

    public void KingPiece()
    {
        State = PieceState.King;
    }
}