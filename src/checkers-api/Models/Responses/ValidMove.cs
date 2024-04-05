using checkers_api.Models.GameModels;

namespace checkers_api.Models.Responses;

public class ValidMove
{
    public Location Destination { get; }
    public IEnumerable<Move> MoveSequence { get; }

    public ValidMove(Location destination, IEnumerable<Move> moveSequence)
    {
        Destination = destination;
        MoveSequence = moveSequence;
    }
}