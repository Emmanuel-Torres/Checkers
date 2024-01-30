using System.Diagnostics;
using checkers_api.Helpers;
using checkers_api.Models.GameModels;

namespace checkers_api.GameLogic;

public class Game
{
    private const int PLAYER_1_HOME_ROW = 0;
    private const int PLAYER_2_HOME_ROW = 7;
    private readonly string _id;
    private readonly Player _player1;
    private readonly Player _player2;
    private readonly List<Player> _players;
    private readonly Piece?[] _board;
    public Game(Player player1, Player player2, Piece?[]? startingBoard = null)
    {
        _id = Guid.NewGuid().ToString();
        _player1 = player1;
        _player2 = player2;
        _players = new() { player1, player2 };

        if (startingBoard is null)
        {
            _board = new Piece[64];
            InitBoard(player1.PlayerId, player2.PlayerId);
        }
        else
        {
            _board = startingBoard;
        }
    }

    public string Id => _id;
    public IEnumerable<Player> Players => _players;
    public IEnumerable<Piece?> Board => _board;

    public void MakeMove(string playerId, MoveRequest request)
    {
        ValidateMove(playerId, request);

        var sourceIndex = request.Source.ToIndex();
        var destinationIndex = request.Destination.ToIndex();

        var piece = _board[sourceIndex];
        if (IsKingRow(playerId, request.Destination.Row))
        {
            piece!.KingPiece();
        }

        _board[sourceIndex] = null;
        _board[destinationIndex] = piece;
    }

    private void ValidateMove(string playerId, MoveRequest request)
    {
        var sourceRow = request.Source.Row;
        var sourceColumn = request.Source.Column;
        var destinationRow = request.Destination.Row;
        var destinationColumn = request.Destination.Column;

        var sourceIndex = request.Source.ToIndex();
        var destinationIndex = request.Destination.ToIndex();

        if (sourceRow < 0 || sourceRow > 7 || sourceColumn < 0 || sourceColumn > 7)
        {
            throw new InvalidOperationException($"Invalid move: Source location ({sourceRow},{sourceColumn}) is out of bounds");
        }
        if (destinationRow < 0 || destinationRow > 7 || destinationColumn < 0 || destinationColumn > 7)
        {
            throw new InvalidOperationException($"Invalid move: Destination location ({destinationRow},{destinationColumn}) is out of bounds");
        }

        var piece = _board[sourceIndex];
        if (piece is null)
        {
            throw new InvalidOperationException($"Invalid move: Source location ({sourceRow},{sourceColumn}) does not contain a piece");
        }
        if (piece.OwnerId != playerId)
        {
            throw new InvalidOperationException($"Invalid move: Player {playerId} does not own the piece at source location ({sourceRow},{sourceColumn})");
        }
        if (_board[destinationIndex] is not null)
        {
            throw new InvalidOperationException($"Invalid move: Destination location ({destinationRow},{destinationColumn}) is not empty");
        }

        var temp = GetRowDelta(sourceRow, destinationRow, playerId);
        var rowDelta =  piece.State == PieceState.Regular ? temp : Math.Abs(temp);
        var columnDelta = Math.Abs(sourceColumn - destinationColumn);

        if (rowDelta < 0)
        {
            throw new InvalidOperationException("Invalid move: Regular pieces cannot move backwards");
        }
        if (rowDelta != columnDelta)
        {
            throw new InvalidOperationException($"Invalid move: Pieces can only move diagonally");
        }
        if (rowDelta > 1)
        {
            throw new InvalidOperationException("Invalid move: Pieces can only move one square when not capturing");
        }
    }
    private bool IsKingRow(string playerId, int row)
    {
        return (playerId == _player1.PlayerId && row == PLAYER_2_HOME_ROW) || (playerId == _player2.PlayerId && row == PLAYER_1_HOME_ROW);
    }
    private int GetRowDelta(int sourceRow, int destinationRow, string playerId)
    {
        var directionModifier = playerId == _player1.PlayerId ? -1 : 1;
        return (sourceRow - destinationRow) * directionModifier;
    }
    private void InitBoard(string player1Id, string player2Id)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                
                if (row % 2 == col % 2)
                {
                    var index = row * 8 + col;
                    _board[index] = new Piece(player1Id);
                }
            }
        }

        for (int row = 5; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                
                if (row % 2 == col % 2)
                {
                    var index = row * 8 + col;
                    _board[index] = new Piece(player2Id);
                }
            }
        }
    }
}