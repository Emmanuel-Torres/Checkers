namespace checkers_api.Models.DomainModels;

public class Id : IEquatable<Id>
{
    private readonly string id;
    public string Value { get => id; }

    public Id()
    {
        id = Guid.NewGuid().ToString("N");
    }

    public bool Equals(Id? other)
    {
        return other is not null && other.Value == this.id;
    }
}