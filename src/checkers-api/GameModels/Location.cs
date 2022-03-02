namespace checkers_api.GameModels;

public class Location
{
    public int Column { get; set; }
    public int Row { get; set; }

    public Location(int column, int row)
    {
        Column = column;
        Row = row;
    }
}