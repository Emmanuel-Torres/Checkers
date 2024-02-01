using checkers_api.Models.GameModels;

namespace checkers_api.Models.Requests;

public class MoveRequest
{
    public Location Source { get; set; }
    public Location Destination { get; set; }

    public MoveRequest(Location source, Location destination)
    {
        Source = source;
        Destination = destination;
    }
}