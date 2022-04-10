namespace checkers_api.Models.ExternalModels;

public class ProfileUpdateRequest
{
    public string? BestJoke { get; set; }
    public string? IceCreamFlavor { get; set; }
    public string? Pizza { get; set; }
    public int? Age { get; set; }
    public IFormFile? Picture { get; set; }
}