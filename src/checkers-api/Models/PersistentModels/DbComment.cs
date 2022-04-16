namespace checkers_api.Models.PersistentModels;

public class DbComment
{
    public int? Id { get; }
    public string Content { get; }
    public string UserId { get; }

    public DbComment(int? id, string content, string userId)
    {
        Id = id;
        Content = content;
        UserId = userId;
    }
}