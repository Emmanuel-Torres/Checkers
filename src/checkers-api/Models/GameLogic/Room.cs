using checkers_api.Models.GameModels;

namespace checkers_api.Models.GameLogic;

public class Room
{
    private readonly string _roomId;
    private readonly Player _roomOwner;
    private readonly bool _isPrivate;
    private string? _roomCode;

    public Room(string roomId, Player roomOwner, bool isPrivate = false, string? roomCode = null)
    {
        _roomId = roomId;
        _roomOwner = roomOwner;
        _isPrivate = isPrivate;
        _roomCode = roomCode;
    }

    public string RoomId => _roomId;
    public Player RoomOwner => _roomOwner;
    public bool IsPrivate => _isPrivate;
}