using checkers_api.Models.GameModels;

namespace checkers_api.Models.GameLogic;

public class Room
{
    private readonly string _roomId;
    private readonly Player _roomOwner;
    private Player? _roomGuest;
    private string _roomCode;

    public Room(string roomId, Player roomOwner, string roomCode)
    {
        _roomId = roomId;
        _roomOwner = roomOwner;
        _roomCode = roomCode;
    }

    public string RoomId => _roomId;
    public Player RoomOwner => _roomOwner;
    public Player? RoomGuest => _roomGuest;

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
}