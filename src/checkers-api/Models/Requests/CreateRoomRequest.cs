namespace checkers_api.Models.Requests;

public class RoomOptions
{
    public string? RoomId { get; set; }

    public RoomOptions(string? roomId = null)
    {
        RoomId = roomId;
    }
}