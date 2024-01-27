using checkers_api.Models.GameModels;

namespace checkers_api.Helpers;

public static class LocationExtensions
{
    public static int ToIndex(this Location location)
    {
        return location.Row * 8 + location.Column;
    }
}