using checkers_api.GameLogic;
using checkers_api.Models.GameModels;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TechTalk.SpecFlow;

namespace MyProject.Specs.Steps
{
    [Binding]
    public class GameSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public GameSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"the following board with players (.*) and (.*)")]
        public void GivenTheFollowingBoard(string player1, string player2, string startingBoard)
        {
            var parsedBoard = ParseStringBoardToPieceIEnumerable(startingBoard).ToArray();
            var game = new Game(new Player(player1, player1), new Player(player2, player2), parsedBoard);
            _scenarioContext.Add("currentGame", game);
        }

        [When(@"player (.*) makes a move from (-?\d+),(-?\d+) to (-?\d+),(-?\d+)")]
        public void WhenPlayerMakesAMoveFrom(string player, int startRow, int startColumn, int endRow, int endColumn)
        {
            try 
            {
                var game = _scenarioContext.Get<Game>("currentGame");
                game.MakeMove(player, new MoveRequest((startRow, startColumn), (endRow, endColumn)));
                _scenarioContext["currentGame"] = game;
            }
            catch (Exception ex)
            {
                _scenarioContext.Add("moveException", ex);
            }
        }

        [When(@"I start a game with players (.*) and (.*)")]
        public void WhenIStartAGameWithPlayers(string p1, string p2)
        {
            var game = new Game(new Player(p1, p1), new Player(p2, p2));
            _scenarioContext.Add("currentGame", game);
        }

        [Then(@"the board should look like this")]
        public void ThenTheFollowingBoardShouldGetCreated(string expectedBoard)
        {
            var parsedExpectedBoard = ParseStringBoardToStringArray(expectedBoard);
            var board = _scenarioContext.Get<Game>("currentGame").Board.ToList();

            _scenarioContext.ContainsKey("moveException").Should().BeFalse();
            board.Select(p => p?.ToString()).Should().Equal(parsedExpectedBoard);
        }

        [Then(@"the move should fail with error '(.*)'")]
        public void ThenTheMoveShouldFailWithError(string expectedError)
        {
            var moveException = _scenarioContext.Get<Exception>("moveException");
            moveException.Message.Should().Contain(expectedError);
        }

        [Then(@"the piece at (\d),(\d) should be a king piece")]
        public void ThePieceAtShouldBeAKingPiece(int sourceRow, int sourceColumn)
        {
            var index = sourceRow * 8 + sourceColumn;
            var piece = _scenarioContext.Get<Game>("currentGame").Board.ToArray()[index];

            piece?.State.Should().Be(PieceState.King);
        }

        private IEnumerable<string?> ParseStringBoardToStringArray(string board)
        {
            return board.Split('|').ToList().Select(c => string.IsNullOrWhiteSpace(c) ? null : c.Trim());
        }

        private IEnumerable<Piece?> ParseStringBoardToPieceIEnumerable(string board)
        {
            return ParseStringBoardToStringArray(board).Select(ParsePieceFromString);
        }

        private Piece? ParsePieceFromString(string? id)
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
}