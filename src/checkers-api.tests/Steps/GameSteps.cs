using System.Text.Json;
using checkers_api.Models.GameLogic;
using checkers_api.Models.GameModels;
using checkers_api.Models.Requests;
using checkers_api.tests.Helpers;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace checkers_api.tests.Steps
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
            var parsedBoard = Parser.ParseStringToPieceBoard(startingBoard);
            var game = new Game("game", new Player(player1, player1), new Player(player2, player2), parsedBoard, new Player(currentTurn, currentTurn));
            _scenarioContext.Add("startingBoard", startingBoard);
            _scenarioContext.Add("currentGame", game);
        }

        [When(@"player (.*) makes a move from '(.*)'")]
        public void WhenPlayerMakesAMoveFrom(string player, string request)
        {
            var parsedRequest = Parser.ParseMoveRequestsFromString(request);
            MakeMoves(player, parsedRequest);
        }

        [When(@"I start a game with players (.*) and (.*)")]
        public void WhenIStartAGameWithPlayers(string p1, string p2)
        {
            var game = new Game("game", new Player(p1, p1), new Player(p2, p2));
            _scenarioContext.Add("currentGame", game);
        }

        [Then(@"the board should look like this")]
        public void ThenTheFollowingBoardShouldGetCreated(string expectedBoard)
        {
            var parsedExpectedBoard = Parser.ParseStringToStringBoard(expectedBoard);
            var board = _scenarioContext.Get<Game>("currentGame").Board;

            _scenarioContext.ContainsKey("moveException").Should().BeFalse();
            
            // var actual = board.Select(p => p?.ToString());
            var actual = board.Select(r => r.Select(p => p?.ToString()));

            actual.Should().BeEquivalentTo(parsedExpectedBoard);
        }

        [Then(@"the move should fail with error '(.*)'")]
        public void ThenTheMoveShouldFailWithError(string expectedError)
        {
            var moveException = _scenarioContext.Get<Exception>("moveException");
            
            var startingBoard = _scenarioContext.Get<string>("startingBoard");
            var parsedExpectedBoard = Parser.ParseStringToStringBoard(startingBoard);
            var currentBoard = _scenarioContext.Get<Game>("currentGame").Board;
            
            moveException.Message.Should().Contain(expectedError);

            var actual = currentBoard.Select(r => r.Select(p => p?.ToString()));
            actual.Should().BeEquivalentTo(parsedExpectedBoard);
        }

        [Then(@"player (.*) won the game")]
        public void PlayerWonTheGame(string expectedPlayer)
        {
            var currentGame = _scenarioContext.Get<Game>("currentGame");

            currentGame.IsGameOver.Should().BeTrue();
            currentGame.Winner?.PlayerId.Should().Be(expectedPlayer);
        }

        [Then(@"player (.*) should now be moving")]
        public void PlayerShouldNowBeMoving(string expectedPlayer)
        {
            var currentGame = _scenarioContext.Get<Game>("currentGame");
            var currentTurn = currentGame.CurrentTurn;
            var isGameOver = currentGame.Winner is not null;
            isGameOver.Should().BeFalse();
            currentTurn.PlayerId.Should().Be(expectedPlayer);
        }

        private void MakeMoves(string player, IEnumerable<Move> requests)
        {
            try 
            {
                var game = _scenarioContext.Get<Game>("currentGame");
                game.MakeMove(player, new MoveRequest(requests));
                _scenarioContext["currentGame"] = game;
            }
            catch (Exception ex)
            {
                _scenarioContext.Add("moveException", ex);
            }
        }
    }
}