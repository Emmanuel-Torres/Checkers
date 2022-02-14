using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace checkers_api.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Key]
    public int? Id { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Picture { get; set; }

    [JsonConstructor]
    public User(string email, string firstName, string lastName, string picture, int? id = null)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Picture = picture;
    }
}