using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace checkers_api.Models;

[Index(nameof(Email), IsUnique = true)]
public class UserProfile
{
    [Key]
    public int? Id { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string GivenName { get; set; }
    [Required]
    public string FamilyName { get; set; }
    [Required]
    public string Picture { get; set; }

    [JsonConstructor]
    public UserProfile(string email, string givenName, string familyName, string picture, int? id = null)
    {
        Id = id;
        Email = email;
        GivenName = givenName;
        FamilyName = familyName;
        Picture = picture;
    }
}