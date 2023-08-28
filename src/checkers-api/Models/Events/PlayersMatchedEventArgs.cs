
using checkers_api.Models.GameModels;

namespace checkers_api.Models.Events;

public class PlayersMatchedEventArgs : EventArgs
{
    public Player Player1 { get; }
    public Player Player2 { get; }

    public PlayersMatchedEventArgs(Player player1, Player player2)
    {
        Player1 = player1;
        Player2 = player2;
    }
}