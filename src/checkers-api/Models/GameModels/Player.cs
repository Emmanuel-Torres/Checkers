using checkers_api.Models.DomainModels;

namespace checkers_api.Models.GameModels;
public class Player
{
    private readonly Id playerId;
    private readonly string name;
    public Id PlayerId { get => playerId; }
    public string Name { get => name;  }

    public Player(Id playerId, string name)
    {
        this.playerId = playerId;
        this.name = name;
    }
}
