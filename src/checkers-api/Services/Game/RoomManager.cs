using System.Collections.Concurrent;
using checkers_api.Models.GameLogic;
using checkers_api.Models.GameModels;
using checkers_api.Models.Responses;

namespace checkers_api.Services.RoomManager;

public class RoomManager : IRoomManager
{
    private readonly ILogger<RoomManager> _logger;
    private readonly ConcurrentDictionary<string, Room> _rooms;
    private readonly ConcurrentDictionary<string, string> _playerRoom;

    public RoomManager(ILogger<RoomManager> logger)
    {
        _logger = logger;
        _rooms = new();
        _playerRoom = new();
    }

    public void CreateRoom(Player roomOwner, string roomCode, string? roomId = null)
    {
        roomId ??= Guid.NewGuid().ToString();
        var room = new Room(roomId, roomOwner, roomCode);

        if(_playerRoom.ContainsKey(roomOwner.PlayerId))
        {
            throw new InvalidOperationException("Cannot create room because player is already in a room");
        }

        if(!_rooms.TryAdd(roomId, room))
        {
            throw new InvalidOperationException("Cannot create room because room id already exists");
        }

        _playerRoom.TryAdd(roomOwner.PlayerId, roomId);
    }

    public RoomInfo? GetRoomInfo(string roomId)
    {
        if (!_rooms.TryGetValue(roomId, out var room))
            return null;

        return new RoomInfo(room.RoomId);
    }

    public void JoinRoom(string roomId, Player roomGuest, string roomCode)
    {
        if (!_rooms.ContainsKey(roomId))
        {
            throw new InvalidOperationException("Cannot join room because room does not exist");
        }

        if(_playerRoom.ContainsKey(roomGuest.PlayerId))
        {
            throw new InvalidOperationException("Cannot join room because player is already in a room");
        }

        _rooms[roomId].JoinRoom(roomGuest, roomCode);
        _playerRoom.TryAdd(roomGuest.PlayerId, roomId);
    }

    //     public GameInfo MakeMove(string playerId, MoveRequest moveRequest)
    //     {
    //         throw new NotImplementedException();
    //     }
}
