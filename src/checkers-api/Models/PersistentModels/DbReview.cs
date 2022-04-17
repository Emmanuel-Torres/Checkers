namespace checkers_api.Models.PersistentModels;

public class DbReview
{
    public string Id { get; }
    public string Content { get; }
    public string PlayerId { get; }
    public DateTime PostedOn { get; }

    public DbReview(string id, string content, string userId, DateTime postedOn)
    {
        Id = id;
        Content = content;
        PlayerId = userId;
        PostedOn = postedOn;
    }
}