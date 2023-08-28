using checkers_api.Models.GameModels;

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
    public Task YourTurnToMoveAsync(IEnumerable<Square> board);
    public Task MoveSuccessfulAsync(IEnumerable<Square> board);
    public Task GameOverAsync(string winner, IEnumerable<Square> board);
    public Task SendMessageAsync(string sender, string message);
    public Task SendJoinConfirmationAsync(string name, Color color, IEnumerable<Square> board);
    public Task SendValidMoveLocationsAsync(IEnumerable<Location> validLocations);
}