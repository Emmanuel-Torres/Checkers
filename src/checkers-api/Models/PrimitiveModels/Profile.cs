namespace checkers_api.Models.PrimitiveModels;

public class Profile
{
    public string Id { get; }
    public string Email { get; }
    public string GivenName { get; }
    public string FamilyName { get; }
    public string Picture { get; }

    public Profile(string id, string email, string givenName, string familyName, string picture)
    {
        Id = id;
        Email = email;
        GivenName = givenName;
        FamilyName = familyName;
        Picture = picture;
    }
}