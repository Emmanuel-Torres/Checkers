using checkers_api.Helpers;
using checkers_api.Models.GameModels;
using checkers_api.Models.Requests;
using checkers_api.Models.Responses;

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

    public IEnumerable<ValidMove> GetValidMoves(string playerId, Location source)
    {
        var moves = new List<ValidMove>();
        var piece = _board[source.row][source.column];
        if (piece?.OwnerId != playerId)
        {
            return moves;
        }

        moves.AddRange(GetRegularMoves(playerId, source));
        moves.AddRange(GetAttackMoves(playerId, source, piece.State));
        return moves;
    }
    public void MakeMove(string playerId, MoveRequest request)
    {
        if (_currentTurn.PlayerId != playerId)
        {
            throw new InvalidOperationException($"Player {playerId} tried to move outside its turn");
        }

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
        bool? wasPreviousAttack = null;
        foreach (var request in requests)
        {
            if (wasPreviousAttack == false)
            {
                throw new InvalidOperationException("Piece can only make one regular move per turn");
            }

            var result = ValidateMove(playerId, request, out var isAttackMove);
            if (!result.isValid)
            {
                if (source is not null && destination is not null)
                {
                    _board[source.row][source.column] = initialPiece;
                    _board[destination.row][destination.column] = null;
                }

                throw new InvalidOperationException(result.errorMessage);
            }

            wasPreviousAttack ??= isAttackMove;
            if (wasPreviousAttack != isAttackMove)
            {
                throw new InvalidOperationException("Piece cannot make a regular move after capturing");
            }
            source ??= request.Source;
            destination = request.Destination;

            var piece = initialPiece ??= _board[source.row][source.column]!;

            if (isAttackMove)
            {
                var middleSource = GetMiddleLocation(request.Source, request.Destination);
                toRemove.Add(middleSource);
            }

            if (IsKingRow(playerId, destination!.row) && piece.State is not PieceState.King)
            {
                piece.KingPiece();
            }

            _board[request.Source.row][request.Source.column] = null;
            _board[request.Destination.row][request.Destination.column] = piece;
        }


        foreach (var i in toRemove)
        {
            _board[i.row][i.column] = null;
        }
    }

    private (bool isValid, string? errorMessage) ValidateMove(string playerId, Move request, out bool isAttackMove)
    {
        var sourceRow = request.Source.row;
        var sourceColumn = request.Source.column;
        var destinationRow = request.Destination.row;
        var destinationColumn = request.Destination.column;

        isAttackMove = false;

        if (!IsLocationInBounds(request.Source))
        {
            return (false, $"Source location ({sourceRow},{sourceColumn}) is out of bounds");
        }
        if (!IsLocationInBounds(request.Destination))
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

    private List<ValidMove> GetRegularMoves(string playerId, Location source)
    {
        var moves = new List<ValidMove>();
        var possibleMoves = new List<Location>() { new Location(source.row + 1, source.column + 1),
                                                   new Location(source.row + 1, source.column - 1),
                                                   new Location(source.row - 1, source.column + 1),
                                                   new Location(source.row - 1, source.column - 1) };

        foreach (var d in possibleMoves)
        {
            var move = new Move(source, d);
            var res = ValidateMove(playerId, move, out _);
            if (res.isValid)
            {
                moves.Add(new ValidMove(d, new List<Move>() { move }));
            }
        }

        return moves;
    }

    private List<ValidMove> GetAttackMoves(string playerId, Location source, PieceState state)
    {
        var validMoves = new List<ValidMove>();
        Traverse(playerId, source, source, state, ref validMoves);
        return validMoves;
    }

    private void Traverse(string playerId, Location originalSource, Location currentSource, PieceState state, ref List<ValidMove> validMoves, IEnumerable<Move>? sequence = null, IEnumerable<Location>? previouslyTakenLocations = null)
    {
        previouslyTakenLocations ??= new List<Location>();
        sequence ??= new List<Move>();

        if (state != PieceState.King && IsKingRow(playerId, currentSource.row))
        {
            state = PieceState.King;
        }

        var direction = playerId == _player1.PlayerId ? 2 : -2;
        var possibleLocations = new List<Location>() { new Location(currentSource.row + direction, currentSource.column + 2),
                                                   new Location(currentSource.row + direction, currentSource.column - 2), };

        if (state == PieceState.King)
        {
            possibleLocations.Add(new Location(currentSource.row + (direction * -1), currentSource.column + 2));
            possibleLocations.Add(new Location(currentSource.row + (direction * -1), currentSource.column - 2));

        }

        foreach (var destination in possibleLocations)
        {
            var midLocation = GetMiddleLocation(currentSource, destination);

            if (IsLocationInBounds(destination) &&
                !previouslyTakenLocations.Any(lt => lt.row == midLocation.row && lt.column == midLocation.column) &&
                _board[midLocation.row][midLocation.column] is not null &&
                _board[midLocation.row][midLocation.column]!.OwnerId != playerId &&
                    (_board[destination.row][destination.column] == null ||
                     _board[destination.row][destination.column] == _board[originalSource.row][originalSource.column]))
            {
                var currentLocationsTaken = previouslyTakenLocations.ToList();
                currentLocationsTaken.Add(midLocation);

                var currentSequence = sequence.ToList();
                var move = new Move(currentSource, destination);
                currentSequence.Add(move);

                validMoves.Add(new ValidMove(destination, currentSequence));
                Traverse(playerId, originalSource, destination, state, ref validMoves, currentSequence, currentLocationsTaken);
            }
        }
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
        var rowDiff = Math.Abs(source.row - destination.row);
        if (rowDiff < 2)
        {
            return false;
        }

        var middle = GetMiddleLocation(source, destination);
        return _board[middle.row][middle.column] is not null;
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
        var middleRow = (source.row + destination.row) / 2;
        var middleColumn = (source.column + destination.column) / 2;

        return new Location(middleRow, middleColumn);
    }

    private bool IsLocationInBounds(Location source)
    {
        return source.row >= 0 && source.row <= 7 && source.column >= 0 && source.column <= 7;
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