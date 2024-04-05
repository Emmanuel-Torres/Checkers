using checkers_api.Models.GameLogic;
using checkers_api.Models.Responses;
using checkers_api.tests.Helpers;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace checkers_api.tests.Steps;

[Binding]
public class ValidMovesSteps
{
    private readonly ScenarioContext _scenarioContext;

    public ValidMovesSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [When(@"player (.*) requests the valid moves for location '(.*)'")]
    public void PlayerRequestsValidMovesForLocation(string playerId, string source)
    {
        var parsedSource = Parser.ParseLocationFromString(source);
        var moves = _scenarioContext.Get<Game>("currentGame").GetValidMoves(playerId, parsedSource);
        _scenarioContext.Add("availableMoves", moves);
    }

    [Then(@"the following moves should be available")]
    public void TheFollowingMovesShouldBeAvailable(Table table)
    {
        var expectedMoves = table.CreateSet<(string Destination, string MoveSequence)>()
                                 .Select(m => new ValidMove(Parser.ParseLocationFromString(m.Destination), Parser.ParseMoveRequestsFromString(m.MoveSequence)));

        var actualMoves = _scenarioContext.Get<IEnumerable<ValidMove>>("availableMoves");

        actualMoves.Should().BeEquivalentTo(expectedMoves);
    }

    [Then(@"no valid moves should be available")]
    public void NoValidMovesShouldBeAvailable()
    {
        _scenarioContext.Get<IEnumerable<ValidMove>>("availableMoves").Should().BeEmpty();
    }
}