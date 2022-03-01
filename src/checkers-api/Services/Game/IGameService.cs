using checkers_api.Models;

namespace checkers_api.Services;

public interface IGameService
{
    Task<Player> MatchMakeAsync(string? token);
}