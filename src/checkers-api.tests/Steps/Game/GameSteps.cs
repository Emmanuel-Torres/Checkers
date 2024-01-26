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

        [When(@"I start a game with players (.*) and (.*)")]
        public void WhenIStartAGameWithPlayers(string p1, string p2)
        {
            var board = new Game(new Player(p1, p1), new Player(p2, p2)).Board;
            _scenarioContext.Add("board", board);
        }

        [Then(@"the following board should get created")]
        public void ThenTheFollowingBoardShouldGetCreated(string expectedBoard)
        {
            var parsedExpectedBoard = expectedBoard.Split('|').ToList().Select(c => string.IsNullOrWhiteSpace(c) ? null : c.Trim());
            var board = _scenarioContext.Get<IEnumerable<Piece?>>("board").ToList();

            board.Select(p => p?.OwnerId).Should().Equal(parsedExpectedBoard);
        }
    }
}