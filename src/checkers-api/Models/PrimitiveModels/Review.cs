namespace checkers_api.Models.PrimitiveModels;

public class Review
{
    public string Id { get; }
    public string PlayerName { get; }
    public string Content { get; }
    public DateTime PostedOn { get; }

    public Review(string id, string playerName, string content, DateTime postedOn)
    {
        Id = id;
        PlayerName = playerName;
        Content = content;
        PostedOn = postedOn;
    }
}