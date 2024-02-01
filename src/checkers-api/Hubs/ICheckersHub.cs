using checkers_api.Models.GameModels;
using checkers_api.Models.Requests;

namespace checkers_api.Hubs;

public interface ICheckersHub
{
    //Server Methods
    public Task FindGameAsync();
    public Task MakeMoveAsync(MoveRequest moveRequest);
    public Task GetValidMovesAsync(Location source);
    public Task QuitGameAsync();
    public Task MoveCompletedAsync();

    //Client methods
    public Task YourTurnToMoveAsync(IEnumerable<Piece?> board);
    public Task MoveSuccessfulAsync(IEnumerable<Piece?> board);
    public Task GameOverAsync(string winner, IEnumerable<Piece?> board);
    public Task SendMessageAsync(string sender, string message);
    public Task SendJoinConfirmationAsync(string name, IEnumerable<Piece?> board);
    public Task SendValidMoveLocationsAsync(IEnumerable<Location> validLocations);
}