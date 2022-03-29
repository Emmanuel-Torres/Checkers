using checkers_api.Models.DomainModels;

namespace checkers_api.Models.GameModels;
public class Player
{
    private readonly Id playerId;
    private readonly string name;
    public Id PlayerId { get => playerId; }
    public string Name { get => name;  }

    public Player(string playerId, string name)
    {
        this.playerId = new Id(playerId);
        this.name = name;
    }
}
