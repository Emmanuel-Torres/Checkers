namespace checkers_api.Models.GameModels;
public class Player
{
    public string PlayerId { get; }
    public string Name { get; }
    public Player(string playerId, string name)
    {
        PlayerId = playerId;
        Name = name;
    }
}
