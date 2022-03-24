using checkers_api.Models.DomainModels;

namespace checkers_api.Models.GameModels;

public class Piece
{
    public readonly Color Color;
    public readonly Id OwnerId;
    public PieceState State { get; private set; }

    public Piece(Color color, Id ownerId)
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