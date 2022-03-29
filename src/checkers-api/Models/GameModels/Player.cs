namespace checkers_api.Models.GameModels;
public class Player
{
    private readonly string playerId;
    private readonly string name;
    public string PlayerId { get => playerId; }
    public string Name { get => name;  }

    public Player(string playerId, string name)
    {
        this.playerId = playerId;
        this.name = name;
    }
}
