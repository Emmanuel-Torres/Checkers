using checkers_api.Models.GameModels;

namespace checkers_api.Helpers;

public static class IntExtensions
{
    public static Location ToLocation(this int index)
    {
        if (index < 0 || index > 63)
        {
            throw new ArgumentException("Not a valid index");
        }

        int row = index / 8;
        int column = index % 8;

        return new Location(row, column);
    }
}