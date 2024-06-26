using checkers_api.Models.GameLogic;
using checkers_api.Models.GameModels;
using checkers_api.Models.Responses;
using checkers_api.Services;
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
        var mockGenerator = new Mock<ICodeGenerator>();
        var roomManager = new RoomManager(mockLogger.Object, mockGenerator.Object);
        _scenarioContext.Add("roomManager", roomManager);
    }

    [Scope(Feature = "Room Manager")]
    [Given(@"player (.*) and player (.*) are in room '(.*)'")]
    public void PlayerAndPlayerAreInRoom(string player1Id, string player2Id, string roomId)
    {
        CreateRoom(player1Id, roomId);
        JoinRoom(player2Id, roomId);
    }

    [When(@"player (.*) creates room '(.*)'")]
    public void PlayerCreatesRoom(string playerId, string roomId)
    {
        try
        {
            CreateRoom(playerId, roomId);
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
        var rooms = table.CreateSet<(string Player, string RoomId)>();
        foreach (var r in rooms)
        {
            PlayerCreatesRoom(r.Player, r.RoomId);
        }
    }

    [Scope(Feature = "Room Manager")]
    [When(@"player (.*) tries to join room '(.*)'")]
    public void PlayerTriesToJoinRoom(string playerId, string roomId)
    {
        try
        {
            JoinRoom(playerId, roomId);
        }
        catch (Exception ex)
        {
            _scenarioContext.Add("actionException", ex);
        }
    }

    [Scope(Feature = "Room Manager")]
    [When(@"player (.*) tries to kick the guest player of room '(.*)'")]
    public void PlayerTriesToKickPlayerFromRoom(string requestorId, string roomId)
    {
        try
        {
            KickGuestPlayer(roomId, requestorId);
        }
        catch(Exception ex)
        {
            _scenarioContext.Add("actionException", ex);
        }
    }

    [When(@"room '(.*)' gets removed")]
    public void RoomGetsRemoved(string roomId)
    {
        try
        {
            RemoveRoom(roomId);
        }
        catch (Exception ex)
        {
            _scenarioContext.Add("actionException", ex);
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

        foreach (var id in expectedRoomIds)
        {
            actualRooms.Add(GetRoomInfo(id));
        }

        actualRooms.Select(r => r?.RoomId).Should().Equal(expectedRoomIds);
    }

    [Scope(Feature = "Room Manager")]
    [Then(@"player (.*) successfully joined room '(.*)'")]
    public void PlayerSuccessfullyJoinedRoom(string expectedPlayerId, string expectedRoomId)
    {
        var roomInfo = GetRoomInfo(expectedRoomId);
        roomInfo?.RoomGuest?.PlayerId.Should().Be(expectedPlayerId);
    }

    [Then(@"player (.*) now exists in the player-room list")]
    public void PlayerNowExistsInThePlayerRoomList(string playerId)
    {
        PlayerExists(playerId).Should().BeTrue();
    }

    [Then(@"room '(.*)' should not exist")]
    public void RoomShouldNotExist(string roomId)
    {
        GetRoomInfo(roomId).Should().BeNull();
    }

    [Then(@"player (.*) should not exists in player-room list")]
    public void ThenPlayerShouldNotExistInPlayerRoomList(string playerId)
    {
        PlayerExists(playerId).Should().BeFalse();
    }

    private RoomInfo? GetRoomInfo(string roomId)
    {
        return _scenarioContext.Get<RoomManager>("roomManager").GetRoomInfo(roomId);
    }

    private void CreateRoom(string playerId, string roomId)
    {
        ((RoomManager)_scenarioContext["roomManager"]).CreateRoom(new Player(playerId, playerId), roomId);
    }

    private void JoinRoom(string playerId, string roomId)
    {
        ((RoomManager)_scenarioContext["roomManager"]).JoinRoom(roomId, new Player(playerId, playerId));
    }

    private void KickGuestPlayer(string roomId, string playerId)
    {
        ((RoomManager)_scenarioContext["roomManager"]).KickGuestPlayer(playerId);
    }

    private void RemoveRoom(string roomId)
    {
        ((RoomManager)_scenarioContext["roomManager"]).RemoveRoom(roomId);
    }

    private bool PlayerExists(string playerId)
    {
        return _scenarioContext.Get<RoomManager>("roomManager").PlayerExists(playerId);
    }
}