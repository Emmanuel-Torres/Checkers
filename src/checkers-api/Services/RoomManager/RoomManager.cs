using System.Collections.Concurrent;
using checkers_api.Models.GameLogic;
using checkers_api.Models.GameModels;
using checkers_api.Models.Requests;
using checkers_api.Models.Responses;

namespace checkers_api.Services;

public class RoomManager : IRoomManager
{
    private readonly ILogger<RoomManager> _logger;
    private readonly ICodeGenerator _codeGenerator;
    private readonly ConcurrentDictionary<string, Room> _rooms;
    private readonly ConcurrentDictionary<string, string> _playerRoom;

    public RoomManager(ILogger<RoomManager> logger, ICodeGenerator codeGenerator)
    {
        _logger = logger;
        _codeGenerator = codeGenerator;
        _rooms = new();
        _playerRoom = new();
    }

    public RoomInfo CreateRoom(Player roomOwner, string? roomId = null)
    {
        roomId ??= _codeGenerator.GenerateCode();

        var room = new Room(roomId, roomOwner);

        if (_playerRoom.ContainsKey(roomOwner.PlayerId))
        {
            throw new InvalidOperationException("Cannot create room because player is already in a room");
        }

        if (!_rooms.TryAdd(roomId, room))
        {
            throw new InvalidOperationException("Cannot create room because room id already exists");
        }

        _playerRoom.TryAdd(roomOwner.PlayerId, roomId);
        return new RoomInfo(roomId, roomOwner);
    }

    public RoomInfo JoinRoom(string roomId, Player roomGuest)
    {
        if (_playerRoom.ContainsKey(roomGuest.PlayerId))
        {
            throw new InvalidOperationException("Cannot join room because player is already in a room");
        }

        ValidateRoomExists(roomId, "Join Room");
        _rooms[roomId].JoinRoom(roomGuest);
        _playerRoom.TryAdd(roomGuest.PlayerId, roomId);

        return new RoomInfo(roomId, _rooms[roomId].RoomOwner, roomGuest);
    }

    public RoomInfo? RemoveRoom(string roomId)
    {
        if (!_rooms.ContainsKey(roomId))
        {
            return null;
        }

        if (!_rooms.TryRemove(roomId, out var room))
        {
            return null;
        }

        var roomOwnerId = room.RoomOwner.PlayerId;
        var roomGuest = room.RoomGuest?.PlayerId;

        _playerRoom.TryRemove(roomOwnerId, out var _);
        if (roomGuest is not null)
        {
            _playerRoom.TryRemove(roomGuest, out var _);
        }

        return new RoomInfo(room.RoomId, room.RoomOwner, room.RoomGuest);
    }

    public GameInfo StartGame(string requestorId)
    {
        var roomId = GetRoomIdByPlayerId(requestorId) ?? throw new InvalidOperationException("Cannot start a game because player is not in a room");
        ValidateRoomExists(roomId, "Start Game");
        return _rooms[roomId].StartGame(requestorId);
    }

    public GameInfo MakeMove(string requestorId, MoveRequest request)
    {
        var roomId = GetRoomIdByPlayerId(requestorId) ?? throw new InvalidOperationException("Cannot make move because player is not in a room");
        ValidateRoomExists(roomId, "Make Move");
        return _rooms[roomId].MakeMove(requestorId, request);
    }

    public (RoomInfo roomInfo, Player? kickedPlayer) KickGuestPlayer(string requestorId)
    {
        var roomId = GetRoomIdByPlayerId(requestorId) ?? throw new InvalidOperationException("Cannot complete action because player is not in a room");
        ValidateRoomExists(roomId, "Kick Guest Player");

        var guest = _rooms[roomId].RoomGuest;
        var guestId = guest?.PlayerId;
        if (guestId is not null)
        {
            _rooms[roomId].KickGuestPlayer(requestorId);
            _playerRoom.TryRemove(guestId, out var _);
        }

        return (GetRoomInfo(roomId)!, guest);
    }

    public RoomInfo? GetRoomInfo(string roomId)
    {
        if (!_rooms.TryGetValue(roomId, out var room))
            return null;

        return new RoomInfo(room.RoomId, room.RoomOwner, room.RoomGuest);
    }

    public bool PlayerExists(string playerId)
    {
        return _playerRoom.ContainsKey(playerId);
    }

    private void ValidateRoomExists(string roomId, string action)
    {
        if (!_rooms.ContainsKey(roomId))
        {
            throw new InvalidOperationException($"Cannot complete action <{action}> because room does not exist");
        }
    }

    private string? GetRoomIdByPlayerId(string playerId)
    {
        _playerRoom.TryGetValue(playerId, out var roomId);
        return roomId;
    }
}
