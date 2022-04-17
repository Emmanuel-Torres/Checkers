namespace checkers_api.Models.PrimitiveModels;

public class Review
{
    public string Id { get; }
    public string Content { get; }
    public string? PlayerId { get; }
    public DateTime PostedOn { get; }

    public Review(string id, string? playerId, string content, DateTime postedOn)
    {
        Id = id;
        PlayerId = playerId;
        Content = content;
        PostedOn = postedOn;
    }
}