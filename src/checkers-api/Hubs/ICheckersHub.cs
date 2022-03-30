using checkers_api.Models.GameModels;

namespace checkers_api.Hubs;

public interface ICheckersHub
{
    //Server Methods
    Task MatchMakeAsync(string? token);
    Task MakeMoveAsync(MoveRequest moveRequest);
    Task GetValidMovesAsync(Location source);
    Task QuitGameAsync();
    Task YourTurnToMoveAsync();
    Task SendValidMoveLocationsAsync();
    Task MoveCompletedAsync();

    //Client methods
    Task MoveSuccessfulAsync(IEnumerable<Square> board);
    Task GameOverAsync(GameResults results);
    Task SendMessageAsync(string sender, string message);
    Task SendJoinConfirmationAsync(string name, IEnumerable<Square> board);
}