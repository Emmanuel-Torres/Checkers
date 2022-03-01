namespace checkers_api.Hubs;

public interface ICheckersHub
{
    Task MatchMakeAsync(string token);
}