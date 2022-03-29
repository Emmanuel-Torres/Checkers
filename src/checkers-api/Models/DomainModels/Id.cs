namespace checkers_api.Models.DomainModels;

public class Id : IEquatable<Id>
{
    private readonly string id;
    public string Value { get => id; }

    public Id()
    {
        id = Guid.NewGuid().ToString("N");
    }

    public Id (string id)
    {
        ArgumentNullException.ThrowIfNull(id);
        if (!IsIdValid(id))
        {
            throw new ArgumentException("Id was not valid");
        }

        this.id = id;
    }

    public bool Equals(Id? other)
    {
        return other is not null && other.Value == this.id;
    }

    private bool IsIdValid(string id)
    {
        return true;
    }
}