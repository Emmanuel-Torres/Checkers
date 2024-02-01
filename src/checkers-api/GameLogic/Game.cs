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
    private Player _currentTurn;
    public Game(Player player1, Player player2, Piece?[]? startingBoard = null, Player? startingPlayer = null)
    {
        _id = Guid.NewGuid().ToString();
        _player1 = player1;
        _player2 = player2;
        _players = new() { player1, player2 };
        
        if (startingPlayer is null)
        {
            _currentTurn = player1;
        }
        else
        {
            _currentTurn = startingPlayer;
        }

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
    public Player CurrentTurn => _currentTurn;

    public void MakeMove(string playerId, IEnumerable<MoveRequest> requests)
    {
        var toRemove = new List<int>();
        Location? source = null;
        Location? destination = null;
        Piece? initialPiece = null;

        foreach(var request in requests)
        {
            try 
            {
                ValidateMove(playerId, request, out var isAttackMove);
                source ??= request.Source;
                destination = request.Destination;

                var sourceIndex = request.Source.ToIndex();
                var destinationIndex = request.Destination.ToIndex();
            
                var piece = initialPiece??= _board[sourceIndex]!;

                if (isAttackMove)
                {
                    var middleSource = (sourceIndex + destinationIndex) / 2;
                    toRemove.Add(middleSource);
                }

                if (IsKingRow(playerId, destination!.Row) && piece.State is not PieceState.King)
                {
                    piece.KingPiece();
                }

                _board[sourceIndex] = null;
                _board[destinationIndex] = piece;
            }
            catch
            {
                if (source is not null && destination is not null)
                {
                    _board[source.ToIndex()] = initialPiece;
                    _board[destination.ToIndex()] = null;
                }
                throw;
            }
        }


        foreach(var i in toRemove)
        {
            _board[i] = null;
        }

        _currentTurn = _currentTurn.PlayerId == _player1.PlayerId ? _player2 : _player1;
    }

    private void ValidateMove(string playerId, MoveRequest request, out bool isAttackMove)
    {
        var sourceRow = request.Source.Row;
        var sourceColumn = request.Source.Column;
        var destinationRow = request.Destination.Row;
        var destinationColumn = request.Destination.Column;

        var sourceIndex = request.Source.ToIndex();
        var destinationIndex = request.Destination.ToIndex();

        if (_currentTurn.PlayerId != playerId)
        {
            throw new InvalidOperationException($"Player {playerId} tried to move outside its turn");
        }
        if (sourceRow < 0 || sourceRow > 7 || sourceColumn < 0 || sourceColumn > 7)
        {
            throw new InvalidOperationException($"Source location ({sourceRow},{sourceColumn}) is out of bounds");
        }
        if (destinationRow < 0 || destinationRow > 7 || destinationColumn < 0 || destinationColumn > 7)
        {
            throw new InvalidOperationException($"Destination location ({destinationRow},{destinationColumn}) is out of bounds");
        }

        var piece = _board[sourceIndex];
        if (piece is null)
        {
            throw new InvalidOperationException($"Source location ({sourceRow},{sourceColumn}) does not contain a piece");
        }
        if (piece.OwnerId != playerId)
        {
            throw new InvalidOperationException($"Player {playerId} does not own the piece at source location ({sourceRow},{sourceColumn})");
        }
        if (_board[destinationIndex] is not null)
        {
            throw new InvalidOperationException($"Destination location ({destinationRow},{destinationColumn}) is not empty");
        }

        var temp = GetRowDelta(sourceRow, destinationRow, playerId);
        var rowDelta =  piece.State == PieceState.Regular ? temp : Math.Abs(temp);
        var columnDelta = Math.Abs(sourceColumn - destinationColumn);

        if (rowDelta < 0)
        {
            throw new InvalidOperationException("Regular pieces cannot move backwards");
        }
        if (rowDelta != columnDelta)
        {
            throw new InvalidOperationException($"Pieces can only move diagonally");
        }
        
        var isPlayerCapturing = IsPlayerCapturing(sourceIndex, destinationIndex);
        if (rowDelta > 2 || (rowDelta > 1 && !isPlayerCapturing))
        {
            throw new InvalidOperationException("Pieces can only move one square when not capturing");
        }

        isAttackMove = isPlayerCapturing;
    }
    private bool IsKingRow(string playerId, int row)
    {
        return (playerId == _player1.PlayerId && row == PLAYER_2_HOME_ROW) || (playerId == _player2.PlayerId && row == PLAYER_1_HOME_ROW);
    }

    private bool IsPlayerCapturing(int sourceIndex, int destinationIndex)
    {
        var middleIndex = (sourceIndex + destinationIndex) / 2;
        return _board[middleIndex] is not null;
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