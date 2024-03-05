using checkers_api.Models.GameModels;
using checkers_api.Models.Requests;
using checkers_api.Models.Responses;

namespace checkers_api.Services.RoomManager;

public interface IRoomManager
{
    public RoomInfo CreateRoom(Player roomOwner, string roomCode, string? roomId = null);
    public RoomInfo JoinRoom(string roomId, Player roomGuest, string roomCode);
    public GameInfo StartGame(string roomId, string requestorId);
    public GameInfo MakeMove(string roomId, string requestorId, IEnumerable<MoveRequest> requests);
    public void KickGuestPlayer(string roomId, string requestorId);
    public RoomInfo? GetRoomInfo(string roomId);
    public bool PlayerExists(string playerId);

    // public GameInfo MakeMove(string playerId, MoveRequest moveRequest);
    // public string StartGame(Player player1, Player player2);
    // public Game? GetGameByGameId(string gameId);
    // public Game? GetGameByPlayerId(string playerId);
    // public GameResults TerminateGame(string gameId);
    // public GameResults QuitGame(string playerId);
    // public IEnumerable<Location> GetValidMoves(string playerId, Location location);
}