using checkers_api.Models.GameModels;
using checkers_api.Models.Requests;

namespace checkers_api.tests.Helpers;

public static class Parser
{
    public static IEnumerable<MoveRequest> ParseMoveRequestsFromString(string request)
    {
        var locations = request.Split('>');

        if (locations.Length < 2)
        {
            throw new InvalidOperationException("Invalid string for move request");
        }

        var requests = new List<MoveRequest>();

        for (int i = 0; i < locations.Length - 1; i++)
        {
            var source = ParseLocationFromString(locations[i]);
            var destination = ParseLocationFromString(locations[i + 1]);

            requests.Add(new MoveRequest(source, destination));
        }

        return requests;
    }

    public static IEnumerable<string?> ParseStringBoardToStringArray(string board)
    {
        return board.Split('|').ToList().Select(c => string.IsNullOrWhiteSpace(c) ? null : c.Trim());
    }

    public static IEnumerable<Piece?> ParseStringBoardToPieceIEnumerable(string board)
    {
        return ParseStringBoardToStringArray(board).Select(ParsePieceFromString);
    }

    public static Location ParseLocationFromString(string location)
    {
        var split = location.Split(',').Select(Int32.Parse).ToArray();
        return new Location(split[0], split[1]);
    }

    public static Piece? ParsePieceFromString(string? id)
    {
        if (id is null)
            return null;

        if (id.Contains('$'))
        {
            var piece = new Piece(id.Trim('$'));
            piece.KingPiece();
            return piece;
        }

        return new Piece(id);
    }
}