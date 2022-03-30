namespace checkers_api.Models.GameModels;

public class Square
{
    public Location Location { get; }
    public Color Color { get; }
    public Piece? Piece { get; set; }
    public bool IsOccupied => Piece is not null;

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