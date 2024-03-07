using checkers_api.Models.Requests;
using checkers_api.Models.Responses;

namespace checkers_api.Hubs;

public interface ICheckersHub
{
    //Server Methods
    public Task CreateRoomAsync(string ownerName, RoomOptions? options = null);
    public Task JoinRoomAsync(string roomId, string guestName);

    //Client methods
    public Task SendRoomInfoAsync(RoomInfo roomInfo);

    // public Task MakeMoveAsync(MoveRequest moveRequest);
    // public Task MoveCompletedAsync();

    // public Task YourTurnToMoveAsync(IEnumerable<Piece?> board);
    // public Task MoveSuccessfulAsync(IEnumerable<Piece?> board);
    // public Task GameOverAsync(string winner, IEnumerable<Piece?> board);
    // public Task SendMessageAsync(string sender, string message);
    // public Task SendJoinConfirmationAsync(string name, IEnumerable<Piece?> board);
    // public Task SendValidMoveLocationsAsync(IEnumerable<Location> validLocations);
}