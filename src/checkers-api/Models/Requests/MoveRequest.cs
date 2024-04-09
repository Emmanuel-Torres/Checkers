using checkers_api.Models.GameModels;

namespace checkers_api.Models.Requests;

public class MoveRequest
{
    public IEnumerable<Move> Moves;

    public MoveRequest(IEnumerable<Move> moves)
    {
        Moves = moves;
    }
}