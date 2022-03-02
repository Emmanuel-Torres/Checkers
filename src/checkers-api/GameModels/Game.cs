namespace checkers_api.GameModels;

public class Game : IGame 
{
    public GameResults? GetGameResults()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Location> GetValidMoves(string playerId, Location source)
    {
        throw new NotImplementedException();
    }

    public bool IsGameOver()
    {
        throw new NotImplementedException();
    }

    public void MakeMove(string playerId, MoveRequest moveRequest)
    {
        throw new NotImplementedException();
    }
}