using checkers_api.Services;
using Microsoft.AspNetCore.SignalR;

namespace checkers_api.Hubs;

public class CheckersHub : Hub<ICheckersHub>
{
    private readonly ILogger<CheckersHub> logger;
    private readonly IGameService gameService;

    public CheckersHub(ILogger<CheckersHub> logger, IGameService gameService)
    {
        this.logger = logger;
        this.gameService = gameService;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public async Task MatchMakeAsync(string token)
    {
        await gameService.MatchMakeAsync(token);
    }
}