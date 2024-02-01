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

        [Given(@"the following board with players (.*) and (.*) and player (.*) is moving")]
        public void GivenTheFollowingBoard(string player1, string player2, string currentTurn, string startingBoard)
        {
            var parsedBoard = ParseStringBoardToPieceIEnumerable(startingBoard).ToArray();
            var game = new Game(new Player(player1, player1), new Player(player2, player2), parsedBoard, new Player(currentTurn, currentTurn));
            _scenarioContext.Add("startingBoard", startingBoard);
            _scenarioContext.Add("currentGame", game);
        }

        [When(@"player (.*) makes a move from '(.*)'")]
        public void WhenPlayerMakesAMoveFrom(string player, string request)
        {
            var parsedRequest = ParseMoveRequestsFromString(request);
            MakeMoves(player, parsedRequest);
        }

        [When(@"I start a game with players (.*) and (.*)")]
        public void WhenIStartAGameWithPlayers(string p1, string p2)
        {
            var game = new Game(new Player(p1, p1), new Player(p2, p2));
            _scenarioContext.Add("currentGame", game);
        }

        [When(@"player (.*) requests the valid moves for location '(.*)'")]
        public void PlayerRequestsTheValidMovesForLocation(string player, string source)
        {
            var currentGame = _scenarioContext.Get<Game>("currentGame");
            var availableMoves = currentGame.GetAvailableMoves(player, ParseLocationFromString(source));

            _scenarioContext.Add("availableMoves", availableMoves);
        }

        [Then(@"the following locations should be returned '(.*)'")]
        public void TheFollowingLocationsShouldBeReturned(string expectedLocations)
        {
            var splitExpectedLocations = string.IsNullOrEmpty(expectedLocations) ? new List<string>() : expectedLocations.Split('-').Select(l => l.Trim());
            var availableMoves = _scenarioContext.Get<IEnumerable<Location>>("availableMoves").Select(l => l.ToString());

            availableMoves.Should().Equal(splitExpectedLocations);
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
            
            var startingBoard = _scenarioContext.Get<string>("startingBoard");
            var parsedExpectedBoard = ParseStringBoardToStringArray(startingBoard);
            var currentBoard = _scenarioContext.Get<Game>("currentGame").Board.ToList();
            
            moveException.Message.Should().Contain(expectedError);
            currentBoard.Select(p => p?.ToString()).Should().Equal(parsedExpectedBoard);
        }

        [Then(@"player (.*) won the game")]
        public void PlayerWonTheGame(string expectedPlayer)
        {
            var isGameOver = _scenarioContext.Get<bool>("isGameOver");
            var currentGame = _scenarioContext.Get<Game>("currentGame");

            isGameOver.Should().BeTrue();
            currentGame.Winner?.PlayerId.Should().Be(expectedPlayer);
        }

        [Then(@"player (.*) should now be moving")]
        public void PlayerShouldNowBeMoving(string expectedPlayer)
        {
            var currentTurn = _scenarioContext.Get<Game>("currentGame").CurrentTurn;
            currentTurn.PlayerId.Should().Be(expectedPlayer);
        }

        private void MakeMoves(string player, IEnumerable<MoveRequest> requests)
        {
            try 
            {
                var game = _scenarioContext.Get<Game>("currentGame");
                var canGameContinue = game.MakeMove(player, requests);
                _scenarioContext["currentGame"] = game;
                _scenarioContext.Add("isGameOver", !canGameContinue);
            }
            catch (Exception ex)
            {
                _scenarioContext.Add("moveException", ex);
            }
        }

        private IEnumerable<string?> ParseStringBoardToStringArray(string board)
        {
            return board.Split('|').ToList().Select(c => string.IsNullOrWhiteSpace(c) ? null : c.Trim());
        }

        private IEnumerable<Piece?> ParseStringBoardToPieceIEnumerable(string board)
        {
            return ParseStringBoardToStringArray(board).Select(ParsePieceFromString);
        }

        private IEnumerable<MoveRequest> ParseMoveRequestsFromString(string request)
        {
            var locations = request.Split('>');

            if (locations.Length < 2)
            {
                throw new InvalidOperationException("Invalid string for move request");
            }

            var requests = new List<MoveRequest>();

            for(int i = 0; i < locations.Length - 1; i++)
            {
                var source = ParseLocationFromString(locations[i]);
                var destination = ParseLocationFromString(locations[i + 1]);

                requests.Add(new MoveRequest(source, destination));
            }

            return requests;
        }

        private Location ParseLocationFromString(string location)
        {
            var split = location.Split(',').Select(Int32.Parse).ToArray();
            return new Location(split[0], split[1]);
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