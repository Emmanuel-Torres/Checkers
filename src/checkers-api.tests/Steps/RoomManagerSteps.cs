using checkers_api.Models.GameLogic;
using checkers_api.Models.GameModels;
using checkers_api.Models.Responses;
using checkers_api.Services.RoomManager;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace checkers_api.tests.Steps;

[Binding]
public class RoomManagerSteps
{
    private readonly ScenarioContext _scenarioContext;

    public RoomManagerSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"a room manager exists")]
    public void ARoomManagerExists()
    {
        var mockLogger = new Mock<ILogger<RoomManager>>();
        var roomManager = new RoomManager(mockLogger.Object);
        _scenarioContext.Add("roomManager", roomManager);
    }

    [When(@"player (.*) creates room '(.*)' with code '(.*)'")]
    public void PlayerCreatesRoomWithCode(string playerId, string roomId, string roomCode)
    {
        try
        {
            ((RoomManager)_scenarioContext["roomManager"]).CreateRoom(new Player(playerId, playerId), roomCode, roomId);
        }
        catch (Exception ex)
        {
            _scenarioContext.Add("actionException", ex);
        }
    }

    [Given(@"the following rooms exist")]
    [When(@"the following rooms are created")]
    public void TheFollowingRoomsAreCreated(Table table)
    {
        var rooms = table.CreateSet<(string Player, string RoomId, string RoomCode)>();
        foreach(var r in rooms)
        {
            PlayerCreatesRoomWithCode(r.Player, r.RoomId, r.RoomCode);
        }
    }

    [Then(@"room '(.*)' should now exist")]
    public void RoomShouldNowExist(string expectedRoomId)
    {
        var roomInfo = GetRoomInfo(expectedRoomId);
        roomInfo?.RoomId.Should().Be(expectedRoomId);
    }

    [Then(@"the following rooms should exist")]
    public void TheFollowingRoomsShouldExist(Table table)
    {
        var expectedRoomIds = table.Rows.Select(r => r.Values.First());
        var actualRooms = new List<RoomInfo?>();

        foreach(var id in expectedRoomIds)
        {
            actualRooms.Add(GetRoomInfo(id));
        }

        actualRooms.Select(r => r?.RoomId).Should().Equal(expectedRoomIds);
    }

    private RoomInfo? GetRoomInfo(string roomId)
    {
        return ((RoomManager)_scenarioContext["roomManager"]).GetRoomInfo(roomId);
    }
}