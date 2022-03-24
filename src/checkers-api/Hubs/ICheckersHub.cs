using checkers_api.Models.GameModels;

namespace checkers_api.Hubs;

public interface ICheckersHub
{
    Task MatchMakeAsync(string token);
    Task MakeMoveAsync(MoveRequest moveRequest);
    Task QuitGameAsync();
    Task YourTurnToMove();
    Task GetValidMoves(Location source);
    Task SendValidMoveLocations();
    Task MoveSuccessful();
    Task MoveCompleted();
    Task GameOver(string winner);
}