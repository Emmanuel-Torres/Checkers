using checkers_api.Models.GameModels;

namespace checkers_api.Models.Responses;

public class RoomInfo
{
    public string RoomId { get; }
    public Player RoomOwner { get; }
    public Player? RoomGuest { get; }

    public RoomInfo(string roomId, Player roomOwner, Player? roomGuest = null)
    {
        RoomId = roomId;
        RoomOwner = roomOwner;
        RoomGuest = roomGuest;
    }
}