namespace checkers_api.Models.GameModels;

public class MoveRequest
{
    public Location Source { get; set; }
    public Location Destination { get; set; }

    public MoveRequest(Location source, Location destination)
    {
        Source = source;
        Destination = destination;
    }

        public MoveRequest((int row, int column) source, (int row, int column) destination)
    {
        Source = new Location(source.row, source.column);
        Destination = new Location(destination.row, destination.column);
    }
}