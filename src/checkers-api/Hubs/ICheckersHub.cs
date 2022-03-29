using checkers_api.Models.GameModels;

namespace checkers_api.Hubs;

public interface ICheckersHub
{
    //Server Methods
    Task MatchMakeAsync(string? token);
    Task MakeMoveAsync(MoveRequest moveRequest);
    Task GetValidMoves(Location source);
    Task QuitGameAsync();
    Task YourTurnToMove();
    Task SendValidMoveLocations();
    Task MoveCompleted();

    //Client methods
    Task MoveSuccessful(IEnumerable<Square> board);
    Task GameOver(GameResults results);
    Task SendMessage(string message);
    Task JoinConfirmation(string name, IEnumerable<Square> board);
}