using checkers_api.Models.GameModels;
using checkers_api.Models.Requests;
using checkers_api.Models.Responses;

namespace checkers_api.Services;

public interface IRoomManager
{
    public RoomInfo CreateRoom(Player roomOwner, string? roomId = null);
    public RoomInfo JoinRoom(string roomId, Player roomGuest);
    public RoomInfo? RemoveRoom(string roomId);
    public GameInfo StartGame(string requestorId);
    public GameInfo MakeMove(string requestorId, MoveRequest request);
    public (RoomInfo roomInfo, Player? kickedPlayer) KickGuestPlayer(string requestorId);
    public RoomInfo? GetRoomInfo(string roomId);
    public bool PlayerExists(string playerId);
}