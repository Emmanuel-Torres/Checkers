using checkers_api.Models.GameModels;
using checkers_api.Models.Requests;
using checkers_api.Services;
using Microsoft.AspNetCore.SignalR;

namespace checkers_api.Hubs;

public class CheckersHub : Hub<ICheckersHub>
{
    private readonly ILogger<CheckersHub> _logger;
    private readonly IRoomManager _roomManager;

    public CheckersHub(ILogger<CheckersHub> logger, IRoomManager roomManager)
    {
        _logger = logger;
        _roomManager = roomManager;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogDebug("[{location}]: Player {connectionId} connected to the server", nameof(CheckersHub), Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    // TODO: How to properly handle player disconnects when they are in a room/game
    // public override async Task OnDisconnectedAsync(Exception? exception)
    // {
    //     if (exception is not null)
    //     {
    //         _logger.LogError("[{location}]: Player {connectionId} disconnected from the sever due to an exception. Ex: {ex}", nameof(CheckersHub), Context.ConnectionId, exception);
    //     }
    //     else
    //     {
    //         _logger.LogDebug("[{location}]: Player {connectionId} disconnected from the server", nameof(CheckersHub), Context.ConnectionId);
    //     }

    //     await base.OnDisconnectedAsync(exception);
    // }

    public async Task CreateRoomAsync(string name, RoomOptions? options = null)
    {
        _logger.LogInformation("Creating new room");
        try
        {
            var roomOwner = new Player(Context.ConnectionId, name);
            var roomInfo = _roomManager.CreateRoom(roomOwner, options?.RoomId);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomInfo.RoomId);
            await Clients.Client(Context.ConnectionId).SendRoomInfoAsync(roomInfo);
        }
        catch (Exception ex)
        {
            //TODO: Better error handling logic and better logging message
            _logger.LogError(ex.Message);
        }
    }

    public async Task JoinRoomAsync(string roomId, string name)
    {
        try
        {
            var roomGuest = new Player(Context.ConnectionId, name);
            var roomInfo = _roomManager.JoinRoom(roomId, roomGuest);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomInfo.RoomId);
            await Clients.Group(roomInfo.RoomId).SendRoomInfoAsync(roomInfo);
        }
        catch (Exception ex)
        {
            //TODO: Better error handling logic and better logging message
            _logger.LogError(ex.Message);
        }
    }

    private async Task StartGameAsync()
    {
        try
        {
            var gameInfo = _roomManager.StartGame(Context.ConnectionId);
            await Clients.Group(gameInfo.RoomId).SendGameInfoAsync(gameInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            // _logger.LogError("[{location}]: Could not start game for players {p1} and {p2}. Ex: {ex}", nameof(CheckersHub), p1.PlayerId, p2.PlayerId, ex);
        }
    }

    public async Task MakeMoveAsync(MoveRequest moveRequest)
    {
        try
        {
            var gameInfo = _roomManager.MakeMove(Context.ConnectionId, moveRequest);
            await Clients.Group(gameInfo.RoomId).SendGameInfoAsync(gameInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            // _logger.LogError("[{location}]: Could not make move. Ex: {ex}", nameof(CheckersHub), ex);
            // await Clients.Client(Context.ConnectionId).SendMessageAsync("server", "Something went wrong when making your move");
        }
    }

    public async Task KickGuestPlayer()
    {
        try
        {
            var (roomInfo, kickedPlayer) = _roomManager.KickGuestPlayer(Context.ConnectionId);
            if (kickedPlayer is not null)
            {
                await Groups.RemoveFromGroupAsync(kickedPlayer.PlayerId, roomInfo.RoomId);
                await Clients.Group(roomInfo.RoomId).SendRoomInfoAsync(roomInfo);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            // _logger.LogError("[{location}]: Could not make move. Ex: {ex}", nameof(CheckersHub), ex);
            // await Clients.Client(Context.ConnectionId).SendMessageAsync("server", "Something went wrong when making your move");
        }
    }
}