namespace checkers_api.Models.GameModels;

public class Location
{
    public int Row { get; }
    public int Column { get; }

    public Location(int row, int column)
    {
        Row = row;
        Column = column;
    }
}