using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace checkers_api.Models.PersistentModels;

[Index(nameof(Email), IsUnique = true)]
public class UserProfile
{
    public int? Id { get; set; }
    public string Email { get; set; }
    public string GivenName { get; set; }
    public string FamilyName { get; set; }
    public string Picture { get; set; }
    public string? BestJoke { get; set; }
    public string? IceCreamFlavor { get; set; }
    public string? Pizza { get; set; }
    public int? Age { get; set; }

    public UserProfile(string email, string givenName, string familyName, string picture, int? id = null)
    {
        Id = id;
        Email = email;
        GivenName = givenName;
        FamilyName = familyName;
        Picture = picture;
    }

    [JsonConstructor]
    public UserProfile(string email, string givenName, string familyName, string picture, string? bestJoke, string? iceCreamFlavor, string? pizza, int? age, int? id = null)
    {
        Id = id;
        Email = email;
        GivenName = givenName;
        FamilyName = familyName;
        Picture = picture;
        BestJoke = bestJoke;
        IceCreamFlavor = iceCreamFlavor;
        Pizza = pizza;
        Age = age;
    }
}