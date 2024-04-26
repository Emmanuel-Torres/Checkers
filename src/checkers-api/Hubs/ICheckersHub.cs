using checkers_api.Models.GameModels;
using checkers_api.Models.Requests;
using checkers_api.Models.Responses;

namespace checkers_api.Hubs;

public interface ICheckersHub
{
    //Server Methods
    public Task CreateRoomAsync(string ownerName, RoomOptions? options = null);
    public Task JoinRoomAsync(string roomId, string guestName);
    public Task StartGameAsync();
    public Task GetValidMovesAsync(Location source);
    public Task MakeMoveAsync(MoveRequest request);
    public Task LeaveRoomAsync();
    public Task KickGuestPlayer();

    //Client methods
    public Task SendRoomInfoAsync(RoomInfo roomInfo);
    public Task SendGameInfoAsync(GameInfo gameInfo);
    public Task SendPlayerInfoAsync(Player player, bool roomOwner);
    public Task SendValidMovesAsync(Location source, IEnumerable<ValidMove> validMoves);
    public Task SendPlayerDisconnectedAsync(Player player, bool youDisconnected);
}