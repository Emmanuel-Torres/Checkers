using checkers_api.Models.GameModels;

namespace checkers_api.GameLogic;

public class Game
{
    private readonly string _id;
    private readonly Player _player1;
    private readonly Player _player2;
    private readonly List<Player> _players;
    private readonly Piece[] board;
    public Game(Player player1, Player player2)
    {
        _id = Guid.NewGuid().ToString();
        _player1 = player1;
        _player2 = player2;
        _players = new() { player1, player2 };
        board = new Piece[64];
        InitBoard(player1.PlayerId, player2.PlayerId);
    }

    public string Id => _id;
    public IEnumerable<Player> Players => _players;
    public IEnumerable<Piece?> Board => board;

    private void InitBoard(string player1Id, string player2Id)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                
                if (row % 2 == col % 2)
                {
                    var index = row * 8 + col;
                    board[index] = new Piece(player1Id);
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
                    board[index] = new Piece(player2Id);
                }
            }
        }
    }
}