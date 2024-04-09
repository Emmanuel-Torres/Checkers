namespace checkers_api.Models.GameModels;

public class Location
{
    public int row { get; }
    public int column { get; }

    public Location(int row, int column)
    {
        this.row = row;
        this.column = column;
    }

    public override string ToString()
    {
        return $"{row},{column}";
    }
}