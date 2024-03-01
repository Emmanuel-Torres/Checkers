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

    [Given(@"player (.*) has a room '(.*)' with code '(.*)'")]
    [When(@"player (.*) creates a room '(.*)' with code '(.*)'")]
    public void PlayerCreatesAPrivateRoom(string playerId, string roomId, string roomCode)
    {
        var room = new Room(roomId, new Player(playerId, playerId), roomCode);
        _scenarioContext.Add(roomId, room);
    }

    [Given(@"player (.*) and player (.*) are in room '(.*)':'(.*)'")]
    public void PlayerAndPlayerAreInRoom(string player1Id, string player2Id, string roomId, string roomCode)
    {
        var p1 = new Player(player1Id, player1Id);
        var p2 = new Player(player2Id, player2Id);
        var room = new Room(roomId, p1, roomCode);
        room.JoinRoom(p2, roomCode);

        _scenarioContext.Add(roomId, room);
    }

    [When(@"player (.*) tries to join room '(.*)' with code '(.*)'")]
    public void PlayerTriesToJoinRoomWithCode(string playerId, string roomId, string roomCode)
    {
        try 
        {
            var room = _scenarioContext.Get<Room>(roomId);
            room.JoinRoom(new Player(playerId, playerId), roomCode);
            _scenarioContext[roomId] = room;
        }
        catch (Exception ex)
        {
            _scenarioContext.Add("actionException", ex);
        }
    }

    [Then(@"room '(.*)' should exist with player (.*) as its owner")]
    public void RoomShouldExist(string expectedRoomId, string expectedPlayerId)
    {
        var room = _scenarioContext.Get<Room>(expectedRoomId);
        room.RoomId.Should().Be(expectedRoomId);
        room.RoomOwner.PlayerId.Should().Be(expectedPlayerId);
    }

    [Then(@"player (.*) successfully joined room '(.*)'")]
    public void PlayerSuccessfullyJoinedRoom(string expectedPlayerId, string expectedRoomId)
    {
        var room = _scenarioContext.Get<Room>(expectedRoomId);
        room.RoomGuest?.PlayerId.Should().Be(expectedPlayerId);
    }

    [Then(@"the action should fail with error '(.*)'")]
    public void TheActionShouldFailWithError(string expectedError)
    {
        var actionException = _scenarioContext.Get<Exception>("actionException");
        actionException?.Message.Should().Be(expectedError);
    }
}