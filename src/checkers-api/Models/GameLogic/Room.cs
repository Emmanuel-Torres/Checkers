using checkers_api.Models.GameModels;
using checkers_api.Models.Requests;
using checkers_api.Models.Responses;

namespace checkers_api.Models.GameLogic;

public class Room
{
    private readonly string _roomId;
    private readonly Player _roomOwner;
    private Player? _roomGuest;
    private Game? _game;

    public Room(string roomId, Player roomOwner)
    {
        _roomId = roomId;
        _roomOwner = roomOwner;
    }

    public string RoomId => _roomId;
    public Player RoomOwner => _roomOwner;
    public Player? RoomGuest => _roomGuest;
    public Game? Game => _game;

    public void JoinRoom(Player guestPlayer)
    {
        if (guestPlayer.PlayerId == _roomOwner.PlayerId)
        {
            throw new InvalidOperationException("Cannot join room because you are already in the room");
        }

        if (_roomGuest is not null)
        {
            throw new InvalidOperationException("Cannot join room because it is already full");
        }

        _roomGuest = guestPlayer;
    }

    public GameInfo StartGame(string requestorId)
    {
        if (_roomGuest is null)
        {
            throw new InvalidOperationException("Cannot start a game with only one player");
        }

        if (requestorId != _roomOwner.PlayerId)
        {
            throw new InvalidOperationException("Room guest cannot start a game");
        }

        if (_game is not null && _game.Winner is null)
        {
            throw new InvalidOperationException("Cannot start a new game while one is already ongoing");
        }

        _game = new Game("game", _roomOwner, _roomGuest);
        return new GameInfo(_roomId, _game.CurrentTurn, _game.Board, _game.Winner);
    }

    public IEnumerable<ValidMove> GetValidMoves(string playerId, Location source)
    {
        if (_game is null)
        {
            throw new InvalidOperationException("Cannot get valid moves because a game has not started");
        }

        if (playerId != _roomOwner.PlayerId && playerId != _roomGuest?.PlayerId)
        {
            throw new InvalidOperationException("Cannot get valid moves because player is not in this room");
        }

        return _game.GetValidMoves(playerId, source);
    }

    public GameInfo MakeMove(string playerId, MoveRequest request)
    {
        if (_game is null)
        {
            throw new InvalidOperationException("Cannot make move because a game has not started");
        }

        if (playerId != _roomOwner.PlayerId && playerId != _roomGuest?.PlayerId)
        {
            throw new InvalidOperationException("Cannot make move because player is not in this room");
        }

        _game.MakeMove(playerId, request);
        return new GameInfo(_roomId, _game.CurrentTurn, _game.Board, _game.Winner);
    }

    public void KickGuestPlayer(string requestorId)
    {
        if (requestorId != _roomOwner.PlayerId)
        {
            throw new InvalidOperationException("Only room owner can kick guest player from room");
        }

        _roomGuest = null;
        _game = null;
    }
}