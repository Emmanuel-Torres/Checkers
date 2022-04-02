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

    public override bool Equals(object? obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Location l = (Location)obj;
            return (Column == l.Column) && (Row == l.Row);
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Column);
    }
}