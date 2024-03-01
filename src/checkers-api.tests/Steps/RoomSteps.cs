using checkers_api.Models.GameLogic;
using checkers_api.Models.GameModels;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace checkers_api.tests.Steps;

[Binding]
public class RoomSteps
{
    private readonly ScenarioContext _scenarioContext;

    public RoomSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [When(@"player (.*) creates a room '(.*)'")]
    public void PlayerCreatesARoom(string playerId, string roomId)
    {
        var room = new Room(roomId, new Player(playerId, playerId));
        _scenarioContext.Add("currentRoom", room);
    }

    [When(@"player (.*) creates a private room '(.*)' with code '(.*)'")]
    public void playerCreatesAPrivateRoom(string playerId, string roomId, string roomCode)
    {
        var room = new Room(roomId, new Player(playerId, playerId), true, roomCode);
        _scenarioContext.Add("currentRoom", room);
    }

    [Then(@"(.*) room '(.*)' should exist with player (.*) as its owner")]
    public void RoomShouldExist(string expectedRoomAccess, string expectedRoomId, string expectedPlayerId)
    {
        var room = _scenarioContext.Get<Room>("currentRoom");
        var isPrivate = expectedRoomAccess == "private";

        room.RoomId.Should().Be(expectedRoomId);
        room.RoomOwner.PlayerId.Should().Be(expectedPlayerId);
        room.IsPrivate.Should().Be(isPrivate);
    }
}