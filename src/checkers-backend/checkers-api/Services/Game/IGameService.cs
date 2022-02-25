using checkers_game;

namespace checkers_api.Services;

public interface IGameService
{
    Task<Player> MatchMakeAsync(string? token);
}