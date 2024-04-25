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
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception is not null)
        {
            _logger.LogError("[{location}]: Player {connectionId} disconnected from the sever due to an exception. Ex: {ex}", nameof(CheckersHub), Context.ConnectionId, exception);
        }
        else
        {
            _logger.LogDebug("[{location}]: Player {connectionId} disconnected from the server", nameof(CheckersHub), Context.ConnectionId);
        }

        await RemoveRoomAndDisconnectPlayersAsync(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task CreateRoomAsync(string name, RoomOptions? options = null)
    {
        _logger.LogInformation("Creating new room");
        try
        {
            var roomOwner = new Player(Context.ConnectionId, name);
            var roomInfo = _roomManager.CreateRoom(roomOwner, options?.RoomId);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomInfo.RoomId);
            await Clients.Client(Context.ConnectionId).SendPlayerInfoAsync(roomInfo.RoomOwner, true);
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
        _logger.LogInformation("Joining room");
        try
        {
            var roomGuest = new Player(Context.ConnectionId, name);
            var roomInfo = _roomManager.JoinRoom(roomId, roomGuest);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomInfo.RoomId);
            await Clients.Client(Context.ConnectionId).SendPlayerInfoAsync(roomInfo.RoomGuest!, false);
            await Clients.Group(roomInfo.RoomId).SendRoomInfoAsync(roomInfo);
        }
        catch (Exception ex)
        {
            //TODO: Better error handling logic and better logging message
            _logger.LogError(ex.Message);
        }
    }

    public async Task StartGameAsync()
    {
        _logger.LogInformation("Starting Game");
        try
        {
            var gameInfo = _roomManager.StartGame(Context.ConnectionId);
            _logger.LogInformation("Created game for room {roomId}", gameInfo.RoomId);
            await Clients.Group(gameInfo.RoomId).SendGameInfoAsync(gameInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            // _logger.LogError("[{location}]: Could not start game for players {p1} and {p2}. Ex: {ex}", nameof(CheckersHub), p1.PlayerId, p2.PlayerId, ex);
        }
    }

    public async Task GetValidMovesAsync(Location source)
    {
        _logger.LogInformation("Getting valid moves for player {playerId}", Context.ConnectionId);
        try
        {
            var moves = _roomManager.GetValidMoves(Context.ConnectionId, source);
            await Clients.Client(Context.ConnectionId).SendValidMovesAsync(source, moves);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    public async Task MakeMoveAsync(IEnumerable<Move> moves)
    {
        _logger.LogInformation("Received move request from player {playerId}", Context.ConnectionId);
        try
        {
            var gameInfo = _roomManager.MakeMove(Context.ConnectionId, new MoveRequest(moves));
            await Clients.Group(gameInfo.RoomId).SendGameInfoAsync(gameInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            // _logger.LogError("[{location}]: Could not make move. Ex: {ex}", nameof(CheckersHub), ex);
            // await Clients.Client(Context.ConnectionId).SendMessageAsync("server", "Something went wrong when making your move");
        }
    }

    public async Task LeaveRoomAsync()
    {
        await RemoveRoomAndDisconnectPlayersAsync(Context.ConnectionId);
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

    private async Task RemoveRoomAndDisconnectPlayersAsync(string disconnectedPlayerId)
    {
        var roomId = _roomManager.GetRoomIdByPlayerId(disconnectedPlayerId);
        if (roomId is not null)
        {
            var roomInfo = _roomManager.RemoveRoom(roomId)!;

            Player disconnectedPlayer = disconnectedPlayerId == roomInfo.RoomGuest?.PlayerId ? roomInfo.RoomGuest : roomInfo.RoomOwner;
            Player? remainingPlayer = disconnectedPlayerId == roomInfo.RoomOwner.PlayerId ? roomInfo.RoomGuest : roomInfo.RoomOwner;

            if (remainingPlayer is not null)
            {
                await Clients.Client(remainingPlayer.PlayerId).SendPlayerDisconnectedAsync(disconnectedPlayer, false);
            }

            await Clients.Client(disconnectedPlayer.PlayerId).SendPlayerDisconnectedAsync(disconnectedPlayer, true);
            
            await Groups.RemoveFromGroupAsync(disconnectedPlayer.PlayerId, roomInfo.RoomId);
            await Groups.RemoveFromGroupAsync(remainingPlayer?.PlayerId ?? "", roomInfo.RoomId);
        }
    }
}