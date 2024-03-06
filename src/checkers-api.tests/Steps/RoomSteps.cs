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

    [Given(@"player (.*) has a room '(.*)'")]
    public void PlayerCreatesAPrivateRoom(string playerId, string roomId)
    {
        var room = new Room(roomId, new Player(playerId, playerId));
        _scenarioContext.Add("currentRoom", room);
    }

    [Scope(Feature = "Room")]
    [Given(@"player (.*) and player (.*) are in room '(.*)'")]
    public void PlayerAndPlayerAreInRoom(string player1Id, string player2Id, string roomId)
    {
        var p1 = new Player(player1Id, player1Id);
        var p2 = new Player(player2Id, player2Id);
        var room = new Room(roomId, p1);
        room.JoinRoom(p2);

        _scenarioContext.Add("currentRoom", room);
    }

    [Given(@"room '(.*)' already has an ongoing game")]
    public void RoomAlreadyHasAnOngoingGame(string roomId)
    {
        var room = _scenarioContext.Get<Room>("currentRoom");
        room.StartGame(room.RoomOwner.PlayerId);
        _scenarioContext["currentRoom"] = room;
    }

    [Scope(Feature = "Room")]
    [When(@"player (.*) tries to join room '(.*)'")]
    public void PlayerTriesToJoinRoom(string playerId, string roomId)
    {
        try 
        {
            var room = _scenarioContext.Get<Room>("currentRoom");
            room.JoinRoom(new Player(playerId, playerId));
            _scenarioContext[roomId] = room;
        }
        catch (Exception ex)
        {
            _scenarioContext.Add("actionException", ex);
        }
    }

    [When(@"player (.*) tries to start a game")]
    public void PlayerTriesToStartAGame(string ownerId)
    {
        try
        {
            var room = _scenarioContext.Get<Room>("currentRoom");
            room.StartGame(ownerId);
        }
        catch (Exception ex)
        {
            _scenarioContext.Add("actionException", ex);
        }
    }

    [Scope(Feature = "Room")]
    [When(@"player (.*) tries to kick the guest player of room '(.*)'")]
    public void PlayerTriesToKickPlayerFromRoom(string requestorId, string roomId)
    {
        try
        {
            var room = _scenarioContext.Get<Room>("currentRoom");
            room.KickGuestPlayer(requestorId);
        }
        catch(Exception ex)
        {
            _scenarioContext.Add("actionException", ex);
        }
    }

    [Then(@"room '(.*)' should exist with player (.*) as its owner")]
    public void RoomShouldExist(string expectedRoomId, string expectedPlayerId)
    {
        var room = _scenarioContext.Get<Room>("currentRoom");
        room.RoomId.Should().Be(expectedRoomId);
        room.RoomOwner.PlayerId.Should().Be(expectedPlayerId);
    }

    [Scope(Feature = "Room")]
    [Then(@"player (.*) successfully joined room '(.*)'")]
    public void PlayerSuccessfullyJoinedRoom(string expectedPlayerId, string expectedRoomId)
    {
        var room = _scenarioContext.Get<Room>("currentRoom");
        room.RoomGuest?.PlayerId.Should().Be(expectedPlayerId);
    }

    [Then(@"a game should now exist for room '(.*)'")]
    public void AGameShouldNowExistForRoom(string roomId)
    {
        var room = _scenarioContext.Get<Room>("currentRoom");
        room.Game.Should().NotBeNull();
    }

    [Then(@"the guest player should no longer be in room '(.*)'")]
    public void PlayerShouldNotBeInRoom(string roomId)
    {
        var room = _scenarioContext.Get<Room>("currentRoom");
        room.RoomGuest.Should().BeNull();
    }

    [Then(@"any ongoing game should be terminated")]
    public void AnyOngoingGameShouldBeTerminated()
    {
        var room = _scenarioContext.Get<Room>("currentRoom");
        room.Game.Should().BeNull();
    }

    [Then(@"the action should fail with error '(.*)'")]
    public void TheActionShouldFailWithError(string expectedError)
    {
        var actionException = _scenarioContext.Get<Exception>("actionException");
        actionException?.Message.Should().Be(expectedError);
    }
}