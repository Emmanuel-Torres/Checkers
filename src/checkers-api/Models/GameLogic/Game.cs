using checkers_api.Helpers;
using checkers_api.Models.GameModels;
using checkers_api.Models.Requests;

namespace checkers_api.Models.GameLogic;

public class Game
{
    private const int PLAYER_1_HOME_ROW = 0;
    private const int PLAYER_2_HOME_ROW = 7;
    private readonly string _id;
    private readonly Player _player1;
    private readonly Player _player2;
    private readonly List<Player> _players;
    private readonly List<List<Piece?>> _board;
    private Player _currentTurn;
    private Player? _winner;
    public Game(string gameId, Player player1, Player player2, List<List<Piece?>>? startingBoard = null, Player? startingPlayer = null)
    {
        _id = gameId;
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
            _board = new() { new Piece?[8].ToList(), new Piece?[8].ToList(), new Piece?[8].ToList(), new Piece?[8].ToList(), new Piece?[8].ToList(), new Piece?[8].ToList(), new Piece?[8].ToList(), new Piece?[8].ToList() };
            InitBoard(player1.PlayerId, player2.PlayerId);
        }
        else
        {
            _board = startingBoard;
        }
    }

    public string Id => _id;
    public IEnumerable<Player> Players => _players;
    public List<List<Piece?>> Board => _board;
    public Player CurrentTurn => _currentTurn;
    public Player? Winner => _winner;
    public bool IsGameOver => _winner is not null;

    public void MakeMove(string playerId, MoveRequest request)
    {
        ProcessMoveRequests(playerId, request.Moves);
        CycleTurn();
        var canGameContinue = CanGameContinue();
        _winner = canGameContinue ? null : GetOppositePlayer();
    }

    private void ProcessMoveRequests(string playerId, IEnumerable<Move> requests)
    {
        var toRemove = new List<Location>();
        Location? source = null;
        Location? destination = null;
        Piece? initialPiece = null;

        foreach (var request in requests)
        {
            var result = ValidateMove(playerId, request, out var isAttackMove);
            if (!result.isValid)
            {
                if (source is not null && destination is not null)
                {
                    _board[source.Row][source.Column] = initialPiece;
                    _board[destination.Row][destination.Column] = null;
                }

                throw new InvalidOperationException(result.errorMessage);
            }
            source ??= request.Source;
            destination = request.Destination;

            var piece = initialPiece ??= _board[source.Row][source.Column]!;

            if (isAttackMove)
            {
                var middleSource = GetMiddleLocation(request.Source, request.Destination);
                toRemove.Add(middleSource);
            }

            if (IsKingRow(playerId, destination!.Row) && piece.State is not PieceState.King)
            {
                piece.KingPiece();
            }

            _board[request.Source.Row][request.Source.Column] = null;
            _board[request.Destination.Row][request.Destination.Column] = piece;
        }


        foreach (var i in toRemove)
        {
            _board[i.Row][i.Column] = null;
        }
    }

    private (bool isValid, string? errorMessage) ValidateMove(string playerId, Move request, out bool isAttackMove)
    {
        var sourceRow = request.Source.Row;
        var sourceColumn = request.Source.Column;
        var destinationRow = request.Destination.Row;
        var destinationColumn = request.Destination.Column;

        isAttackMove = false;

        if (_currentTurn.PlayerId != playerId)
        {
            return (false, $"Player {playerId} tried to move outside its turn");
        }
        if (sourceRow < 0 || sourceRow > 7 || sourceColumn < 0 || sourceColumn > 7)
        {
            return (false, $"Source location ({sourceRow},{sourceColumn}) is out of bounds");
        }
        if (destinationRow < 0 || destinationRow > 7 || destinationColumn < 0 || destinationColumn > 7)
        {
            return (false, $"Destination location ({destinationRow},{destinationColumn}) is out of bounds");
        }

        var piece = _board[sourceRow][sourceColumn];
        if (piece is null)
        {
            return (false, $"Source location ({sourceRow},{sourceColumn}) does not contain a piece");
        }
        if (piece.OwnerId != playerId)
        {
            return (false, $"Player {playerId} does not own the piece at source location ({sourceRow},{sourceColumn})");
        }
        if (_board[destinationRow][destinationColumn] is not null)
        {
            return (false, $"Destination location ({destinationRow},{destinationColumn}) is not empty");
        }

        var temp = GetRowDelta(sourceRow, destinationRow, playerId);
        var rowDelta = piece.State == PieceState.Regular ? temp : Math.Abs(temp);
        var columnDelta = Math.Abs(sourceColumn - destinationColumn);

        if (rowDelta < 0)
        {
            return (false, "Regular pieces cannot move backwards");
        }
        if (rowDelta != columnDelta)
        {
            return (false, $"Pieces can only move diagonally");
        }

        var isPlayerCapturing = IsPlayerCapturing(request.Source, request.Destination);
        if (rowDelta > 2 || (rowDelta > 1 && !isPlayerCapturing))
        {
            return (false, "Pieces can only move one square when not capturing");
        }

        isAttackMove = isPlayerCapturing;
        return (true, null);
    }

    private bool CanGameContinue()
    {
        var remainingPieces = new List<(int row, int column)>();
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                if (_board[r][c]?.OwnerId == _currentTurn.PlayerId)
                {
                    remainingPieces.Add((r, c));
                }
            }
        }

        if (!remainingPieces.Any())
        {
            return false;
        }

        foreach (var p in remainingPieces)
        {
            var possibleMoves = new List<Location>() { new Location(p.row + 1, p.column + 1),
                                                       new Location(p.row + 1, p.column - 1),
                                                       new Location(p.row - 1, p.column + 1),
                                                       new Location(p.row - 1, p.column - 1),
                                                       new Location(p.row + 2, p.column + 2),
                                                       new Location(p.row + 2, p.column - 2),
                                                       new Location(p.row - 2, p.column + 2),
                                                       new Location(p.row - 2, p.column - 2) };

            var source = new Location(p.row, p.column);
            var moveRequests = possibleMoves.Select(l => new Move(source, l));
            foreach (var mr in moveRequests)
            {
                var result = ValidateMove(_currentTurn.PlayerId, mr, out _);
                if (result.isValid)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsKingRow(string playerId, int row)
    {
        return (playerId == _player1.PlayerId && row == PLAYER_2_HOME_ROW) || (playerId == _player2.PlayerId && row == PLAYER_1_HOME_ROW);
    }

    private bool IsPlayerCapturing(Location source, Location destination)
    {
        var middle = GetMiddleLocation(source, destination);
        return _board[middle.Row][middle.Column] is not null;
    }

    private int GetRowDelta(int sourceRow, int destinationRow, string playerId)
    {
        var directionModifier = playerId == _player1.PlayerId ? -1 : 1;
        return (sourceRow - destinationRow) * directionModifier;
    }

    private void CycleTurn()
    {
        _currentTurn = GetOppositePlayer();
    }

    private Player GetOppositePlayer()
    {
        return _currentTurn.PlayerId == _player1.PlayerId ? _player2 : _player1;
    }

    private Location GetMiddleLocation(Location source, Location destination)
    {
        var middleRow = (source.Row + destination.Row) / 2;
        var middleColumn = (source.Column + destination.Column) / 2;

        return new Location(middleRow, middleColumn);
    }

    private void InitBoard(string player1Id, string player2Id)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 8; col++)
            {

                if (row % 2 == col % 2)
                {
                    _board[row][col] = new Piece(player1Id);
                }
            }
        }

        for (int row = 5; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {

                if (row % 2 == col % 2)
                {
                    _board[row][col] = new Piece(player2Id);
                }
            }
        }
    }
}