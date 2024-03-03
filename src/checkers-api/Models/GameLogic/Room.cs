using checkers_api.Models.GameModels;

namespace checkers_api.Models.GameLogic;

public class Room
{
    private readonly string _roomId;
    private readonly Player _roomOwner;
    private readonly string _roomCode;
    private Player? _roomGuest;
    private Game? _game;

    public Room(string roomId, Player roomOwner, string roomCode)
    {
        _roomId = roomId;
        _roomOwner = roomOwner;
        _roomCode = roomCode;
    }

    public string RoomId => _roomId;
    public Player RoomOwner => _roomOwner;
    public Player? RoomGuest => _roomGuest;
    public Game? Game => _game;

    public void JoinRoom(Player guestPlayer, string roomCode)
    {
        if (guestPlayer.PlayerId == _roomOwner.PlayerId)
        {
            throw new InvalidOperationException("Cannot join room because you are already in the room");
        }

        if (_roomGuest is not null)
        {
            throw new InvalidOperationException("Cannot join room because it is already full");
        }

        if (roomCode != _roomCode)
        {
            throw new InvalidOperationException("Cannot join room because code was incorrect");
        }

        _roomGuest = guestPlayer;
    }

    public void StartGame(string requestorId)
    {
        if (_roomGuest is null)
        {
            throw new InvalidOperationException("Cannot start a game with only one player");
        }

        if (requestorId != _roomOwner.PlayerId)
        {
            throw new InvalidOperationException("Room guest cannot start a game");
        }

        if (_game is not null && _game.Winner is null)
        {
            throw new InvalidOperationException("Cannot start a new game while one is already ongoing");
        }

        _game = new Game("game", _roomOwner, _roomGuest);
    }
}