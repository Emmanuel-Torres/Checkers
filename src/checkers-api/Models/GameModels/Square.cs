using checkers_api.Models.Game;

namespace checkers_api.Models.GameModels;

public class Square
{
    public readonly Location Location;
    public readonly Color Color;
    public Piece? Piece { get; set; }
    public string StringColor => Color.ToString();
    public bool IsOcupied => Piece is not null;

    public Square(Location location, Color color)
    {
        Location = location;
        Color = color;
    }

    public Square(Location location, Color color, Piece piece)
    {
        Location = location;
        Color = color;
        Piece = piece;
    }
}