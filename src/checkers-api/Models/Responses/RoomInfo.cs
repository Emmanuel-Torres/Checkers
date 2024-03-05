namespace checkers_api.Models.Responses;

public class RoomInfo
{
    public string RoomId { get; }

    public RoomInfo(string roomId)
    {
        RoomId = roomId;
    }
}