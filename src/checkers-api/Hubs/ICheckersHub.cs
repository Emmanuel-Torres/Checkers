using checkers_api.Models.GameModels;

namespace checkers_api.Hubs;

public interface ICheckersHub
{
    Task MatchMakeAsync(string? token);
    Task MakeMoveAsync(MoveRequest moveRequest);
    Task GetValidMoves(Location source);
    Task QuitGameAsync();
    Task YourTurnToMove();
    Task SendValidMoveLocations();
    Task MoveSuccessful();
    Task MoveCompleted();
    Task GameOver(string winner);
    Task SendMessage(string message);
}