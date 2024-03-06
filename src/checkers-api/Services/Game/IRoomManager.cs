using checkers_api.Models.GameModels;
using checkers_api.Models.Requests;
using checkers_api.Models.Responses;

namespace checkers_api.Services.RoomManager;

public interface IRoomManager
{
    public RoomInfo CreateRoom(Player roomOwner, string roomCode, string? roomId = null);
    public RoomInfo JoinRoom(string roomId, Player roomGuest, string roomCode);
    public RoomInfo? RemoveRoom(string roomId);
    public GameInfo StartGame(string roomId, string requestorId);
    public GameInfo MakeMove(string roomId, string requestorId, IEnumerable<MoveRequest> requests);
    public void KickGuestPlayer(string roomId, string requestorId);
    public RoomInfo? GetRoomInfo(string roomId);
    public bool PlayerExists(string playerId);
}