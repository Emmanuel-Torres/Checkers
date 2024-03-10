namespace checkers_api.Models.GameModels;

public class Move
{
    public Location Source { get; set; }
    public Location Destination { get; set; }

    public Move(Location source, Location destination)
    {
        Source = source;
        Destination = destination;
    }
}