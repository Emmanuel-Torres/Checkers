namespace checkers_api.Models.GameModels;

public class Location
{
    public readonly int Row;
    public readonly int Column;

    public Location(int row, int column)
    {
        Row = row;
        Column = column;
    }
}