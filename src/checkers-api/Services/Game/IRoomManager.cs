using checkers_api.Models.GameModels;
using checkers_api.Models.Requests;
using checkers_api.Models.Responses;

namespace checkers_api.Services.RoomManager;

public interface IRoomManager
{
    public void CreateRoom(Player roomOwner, string roomCode, string? roomId = null);
    public RoomInfo? GetRoomInfo(string roomId);
    public void JoinRoom(string roomId, Player roomGuest, string roomCode);

    // public GameInfo MakeMove(string playerId, MoveRequest moveRequest);
    // public string StartGame(Player player1, Player player2);
    // public Game? GetGameByGameId(string gameId);
    // public Game? GetGameByPlayerId(string playerId);
    // public GameResults TerminateGame(string gameId);
    // public GameResults QuitGame(string playerId);
    // public IEnumerable<Location> GetValidMoves(string playerId, Location location);
}