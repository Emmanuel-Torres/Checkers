using checkers_api.Models.GameModels;

namespace checkers_api.Hubs;

public interface ICheckersHub
{
    //Server Methods
    Task MatchMakeAsync(string? token);
    Task MakeMoveAsync(MoveRequest moveRequest);
    Task GetValidMovesAsync(Location source);
    Task QuitGameAsync();
    Task MoveCompletedAsync();

    //Client methods
    Task YourTurnToMoveAsync(IEnumerable<Square> board);
    Task MoveSuccessfulAsync(IEnumerable<Square> board);
    Task GameOverAsync(GameResults results);
    Task SendMessageAsync(string sender, string message);
    Task SendJoinConfirmationAsync(string name, Color color, IEnumerable<Square> board);
    Task SendValidMoveLocationsAsync(IEnumerable<Location> validLocations);
}